namespace BookUniverse.Client
{
    using System;
    using System.IO;
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
        private readonly ISearchBook _searchBookService;
        private User currentUser = new User();
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public UserAccount(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveService,
            ISearchBook searchBookService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _searchBookService = searchBookService;

            Loaded += UserAccount_Loaded;
            Closed += Window_Closed;

            this.DataContext = currentUser;
            _googleDriveService = googleDriveService;

            InitializeComponent();
            Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked += MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked += MenuControl_FavouriteBooksClicked;


        }

        private void MenuControl_SearchBooksClicked(object sender, EventArgs e)
        {
            BookSearch searchBooks = new BookSearch(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            searchBooks.Show();
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked -= MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked -= MenuControl_FavouriteBooksClicked;
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {

            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            listOfBooks.Show();
            Close();
        }

        private void MenuControl_FavouriteBooksClicked(object sender, EventArgs e)
        {
            FavouriteBooksWindow listOfBooks = new FavouriteBooksWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
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
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
            Close();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
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
            AddBookWindow addBook = new AddBookWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            this.Visibility = Visibility.Hidden;
            addBook.Show();
            Close();
        }
    }
}
