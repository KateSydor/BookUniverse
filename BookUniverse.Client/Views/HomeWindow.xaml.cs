namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
	using BookUniverse.BLL.DTOs;
	using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for HomeWindow.xaml.
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
		private readonly IBookService _bookService;
		private readonly IGoogleDriveService _googleDriveService;
        private User currentUser;
        private string filepath;

        public HomeWindow(IAuthenticationService authenticationService, IUserService userService, IBookService bookService, IGoogleDriveService googleDriveService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _googleDriveService = googleDriveService;

            Loaded += HomeWindow_Loaded;

            this.DataContext = currentUser;

            InitializeComponent();
        }

        private async void HomeWindow_Loaded(object sender, RoutedEventArgs e)
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

                //_bookService.GetUserBooks(currentUser.Email);
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _googleDriveService);
                signInPage.Show();
                Hide();
            }
        }


        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _googleDriveService);
            signInPage.Show();
            Hide();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _googleDriveService);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				(int pageCount, Google.Apis.Drive.v3.Data.File uploadedFile) = await _googleDriveService.UploadFile(filepath);
                AddBookDto addBook = new AddBookDto
                {
					Title = uploadedFile.Name,
					Description = description.Text,
					Author = author.Text,
					CategoryName = category.Text,
					NumberOfPages = pageCount,
					Path = uploadedFile.Id
				};

                _bookService.AddBook(addBook);
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
            dlg.DefaultExt = "All Files|*.*";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                filepath = dlg.FileName;
                tbFilepath.Text = filepath;
            }
        }
    }
}
