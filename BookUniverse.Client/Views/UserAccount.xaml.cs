namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for UserAccount.xaml.
    /// </summary>
    public partial class UserAccount : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private User currentUser;

        public UserAccount(IAuthenticationService authenticationService, IUserService userService, IBookService bookService, IGoogleDriveService googleDriveRepository)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;

            Loaded += UserAccount_Loaded;
            this.DataContext = currentUser;
            _googleDriveRepository = googleDriveRepository;

            InitializeComponent();
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

                }
                else
                {
                    throw new Exception(UtilsConstants.FILE_ERROR);
                }
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _googleDriveRepository);
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
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _googleDriveRepository);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
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
                MessageBox.Show("Changes saved successfully!", string.Empty, MessageBoxButton.OK);
            }
            catch
            {
                MessageBox.Show(UtilsConstants.ERROR);
            }
        }
    }
}
