namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.Client.CustomControls;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Enums;

    /// <summary>
    /// Interaction logic for UserAccount.xaml.
    /// </summary>
    public partial class UserAccount : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveService;
        private User currentUser = new User();
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public UserAccount(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;

            Loaded += UserAccount_Loaded;
            Closed += Window_Closed;

            this.DataContext = currentUser;
            _googleDriveService = googleDriveService;

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

        private async void UserAccount_Loaded(object sender, RoutedEventArgs e)
        {
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

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditUserDto newUser = new EditUserDto
            {
                Username = editUsername.Text,
                Email = editEmail.Text
            };
            try
            {
                await _authenticationService.EditUser(currentUser.Id, newUser);
                currentUser = _authenticationService.CurrentAccount;
                UsernameOnTop.Text = currentUser.Username;
                _notifyWindow.ShowNotification("Changes saved successfully!");
            }
            catch
            {
                _notifyWindow.ShowNotification(UtilsConstants.ERROR);
            }
        }

        private void AddBook(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBook = new AddBookWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService);
            addBook.Show();
            Close();
        }
    }
}
