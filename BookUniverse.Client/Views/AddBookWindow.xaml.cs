namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.Client.CustomControls;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for HomeWindow.xaml.
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveService;
        private User currentUser;
        private string filepath;

        public AddBookWindow(
            IAuthenticationService authenticationService,
            IUserService userService, IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveService = googleDriveService;

            Loaded += AddBookWindow_Loaded;
            Closed += Window_Closed;

            this.DataContext = currentUser;

            InitializeComponent();
            Menu.AllBooksClicked += MenuControl_AllBooksClicked;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {

            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService);
            listOfBooks.Show();
            Close();
        }

        private async void AddBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines(UtilsConstants.FILE_PATH);
                if (lines.Length >= 2)
                {
                    int userId = int.Parse(lines[0]);
                    string userEmail = lines[1];

                    currentUser = await _userService.GetUser(userEmail);
                    username.Text = currentUser.Username;
                }
                else
                {
                    throw new Exception(UtilsConstants.FILE_ERROR);
                }
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService);
                signInPage.Show();
                Hide();
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService);
            homeWindow.Show();
            Close();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService);
            signInPage.Show();
            Close();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService);
            userAccount.Show();
            Close();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category findCategory = await _categoryService.CategoryExists(category.Text);
                (int pageCount, Google.Apis.Drive.v3.Data.File uploadedFile) = await _googleDriveService.UploadFile(filepath);
                AddBookDto addBook = CreateAddBookDto(uploadedFile, pageCount);

                _bookService.AddBook(addBook, findCategory);
                MessageBox.Show("File successfully uploaded");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF Files|*.pdf";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                filepath = dlg.FileName;
                tbFilepath.Text = filepath;
            }
        }

        private AddBookDto CreateAddBookDto(Google.Apis.Drive.v3.Data.File uploadedFile, int pageCount)
        {
            return new AddBookDto
            {
                Title = uploadedFile.Name,
                Description = description.Text,
                Author = author.Text,
                CategoryName = category.Text,
                NumberOfPages = pageCount,
                Path = uploadedFile.WebViewLink,
            };
        }
    }
}
