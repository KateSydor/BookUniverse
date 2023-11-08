namespace BookUniverse.Client
{
    using BookUniverse.Client;
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.GoogleDriveRepository;

    /// <summary>
    /// Interaction logic for HomeWindow.xaml.
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IGoogleDriveRepository _googleDriveRepository;
        private User currentUser;

        public HomeWindow(IAuthenticationService authenticationService, IUserService userService, IGoogleDriveRepository googleDriveRepository)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _googleDriveRepository = googleDriveRepository;

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

                    
                    _googleDriveRepository.GetDriveFiles();
                }
                else
                {
                    throw new Exception(UtilsConstants.FILE_ERROR);
                }
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _googleDriveRepository);
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
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _googleDriveRepository);
            signInPage.Show();
            Hide();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _googleDriveRepository);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }
    }
}
