namespace BookUniverse.Client
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.BLL.DTOValidators.BookValidators;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.Client.CustomControls;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    using FluentValidation.Results;

    /// <summary>
    /// Interaction logic for HomeWindow.xaml.
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveService;
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;
        private readonly ISearchBook _searchBookService;
        private User currentUser;
        private Book currentBook;
        private string filepath;
        private ObservableCollection<string> categories;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public AddBookWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveService,
            ISearchBook searchBookService,
            IFolderService folderService,
            IBookFolderService bookFolderService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveService = googleDriveService;
            _folderService = folderService;
            _bookFolderService = bookFolderService;
            _searchBookService = searchBookService;

            Loaded += AddBookWindow_Loaded;
            Closed += Window_Closed;

            currentBook = new Book();
            this.DataContext = currentBook;

            InitializeComponent();

            categories = new ObservableCollection<string>(_categoryService.GetAllCategories().Select(c => c.CategoryName));
            categories.Add("Add new category");
            category.ItemsSource = categories;

            Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked += MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked += MenuControl_FavouriteBooksClicked;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked -= MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked -= MenuControl_FavouriteBooksClicked;
        }

        private void MenuControl_SearchBooksClicked(object sender, EventArgs e)
        {
            BookSearch searchBooks = new BookSearch(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            searchBooks.Show();
            Close();
        }

        private void MenuControl_FavouriteBooksClicked(object sender, EventArgs e)
        {
            FavouriteBooksWindow listOfBooks = new FavouriteBooksWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            listOfBooks.Show();
            Close();
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {

            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private async void AddBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            SystemCommands.MaximizeWindow(this);
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
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
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            homeWindow.Show();
            Close();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            signInPage.Show();
            Close();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            userAccount.Show();
            Close();
        }

        private void CategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;

            if (comboBox.SelectedItem as string == "Add new category")
            {
                AddCategoryWindow addCategory = new AddCategoryWindow(_categoryService, UpdateCategories);
                addCategory.Show();
                comboBox.SelectedItem = null;
            }
        }

        private void UpdateCategories(string newCategoryName)
        {
            categories.Insert(categories.Count - 1, newCategoryName);
            category.ItemsSource = categories;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category findCategory = await _categoryService.CategoryExists(category.Text);
                (int pageCount, Google.Apis.Drive.v3.Data.File uploadedFile) = await _googleDriveService.UploadFile(filepath);
                AddBookDto addBook = CreateAddBookDto(uploadedFile, pageCount);

                AddBookDtoValidator validator = new AddBookDtoValidator();
                ValidationResult validationResult = validator.Validate(addBook);

                if (validationResult.IsValid)
                {
                    _bookService.AddBook(addBook, findCategory);
                    _notifyWindow.ShowNotification("File successfully uploaded");
                }
                else
                {
                    _notifyWindow.ShowNotification(UtilsConstants.INPUT_VALID_DATA);
                }
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
                Title = title.Text,
                Description = description.Text,
                Author = author.Text,
                CategoryName = category.Text,
                NumberOfPages = pageCount,
                Path = uploadedFile.WebViewLink,
            };
        }
    }
}
