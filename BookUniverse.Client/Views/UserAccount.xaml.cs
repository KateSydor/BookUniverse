namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.DTOValidators.UserValidators;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
    using BookUniverse.Client.CustomControls;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Enums;
    using FluentValidation.Results;

    /// <summary>
    /// Interaction logic for UserAccount.xaml.
    /// </summary>
    public partial class UserAccount : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveService;
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;

        private readonly ISearchBook _searchBookService;
        private User currentUser = new User();
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public UserAccount(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveService,
            ISearchBook searchBookService,
            IFolderService folderService,
            IBookFolderService bookFolderService )
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveService = googleDriveService;
            _folderService = folderService;
            _bookFolderService = bookFolderService;
            _searchBookService = searchBookService;

            Loaded += UserAccount_Loaded;
            Closed += Window_Closed;

            this.DataContext = currentUser;
            _googleDriveService = googleDriveService;

            InitializeComponent();
            Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked += MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked += MenuControl_FavouriteBooksClicked;
            GetCategories();
            GetFolders();
        }

        private void GetCategories()
        {
            try
            {
                Menu_Control.Menu_Categories.ItemsSource = _categoryService.GetAllCategories().Select(category => category.CategoryName).ToList();
            }
            catch
            {
                Menu_Control.Menu_Categories.ItemsSource = new List<string>() { "No categories found" };
            }
        }

        private void GetFolders()
        {
            try
            {
                Menu_Control.Menu_Folders.ItemsSource = _folderService.GetAllFolders().Select(category => category.FolderName).ToList();
            }
            catch
            {
                Menu_Control.Menu_Folders.ItemsSource = new List<string>() { "No folders found" };
            }
        }

        private void MenuControl_SearchBooksClicked(object sender, EventArgs e)
        {
            BookSearch searchBooks = new BookSearch(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            searchBooks.Show();
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked -= MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked -= MenuControl_FavouriteBooksClicked;
            _notifyWindow.Close();
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {
            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private void MenuControl_FavouriteBooksClicked(object sender, EventArgs e)
        {
            FavouriteBooksWindow listOfBooks = new FavouriteBooksWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private async void UserAccount_Loaded(object sender, RoutedEventArgs e)
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
                    UsernameOnTop.Text = currentUser.Username;
                    editUsername.Text = currentUser.Username;
                    editEmail.Text = currentUser.Email;
                    _authenticationService.CurrentAccount = currentUser;
                    if (currentUser.Role != Roles.Admin)
                    {
                        AddBookButton.Visibility = Visibility.Hidden;
                    }
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
                Close();
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

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditUserDto newUser = new EditUserDto
            {
                Username = editUsername.Text,
                Email = editEmail.Text
            };
            try
            {
                EditUserDtoValidator validator = new EditUserDtoValidator();
                ValidationResult validationResult = validator.Validate(newUser);

                if (validationResult.IsValid)
                {
                    await _authenticationService.EditUser(currentUser.Id, newUser);
                    currentUser = _authenticationService.CurrentAccount;
                    UsernameOnTop.Text = currentUser.Username;
                    _notifyWindow.ShowNotification("Changes saved successfully!");
                }
                else
                {
                    _notifyWindow.ShowNotification(UtilsConstants.INPUT_VALID_DATA);
                }
            }
            catch
            {
                _notifyWindow.ShowNotification(UtilsConstants.ERROR);
            }
        }

        private void AddBook(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBook = new AddBookWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, _folderService, _bookFolderService);
            addBook.Show();
            Close();
        }
    }
}
