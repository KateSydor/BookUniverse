namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.DTOs;
    using BookUniverse.BLL.Interfaces;

    /// <summary>
    /// Interaction logic for SignInWindow.xaml.
    /// </summary>
    public partial class SignInWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly LoginDto user;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public SignInWindow(IAuthenticationService authenticationService, IUserService userService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userService = userService;

            user = new LoginDto();
            this.DataContext = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow signUpWindow = new MainWindow(_authenticationService, _userService);
            this.Visibility = Visibility.Hidden;
            signUpWindow.Show();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string pass = password.Password.Trim();

            try
            {
                await _authenticationService.Login(user);
                if (_authenticationService.IsLoggedIn())
                {
                    HomeWindow homePage = new HomeWindow(_authenticationService, _userService);
                    homePage.Show();
                    Hide();
                }
            }
            catch (Exception ex)
            {
                _notifyWindow.ShowNotification("Error: " + ex.Message.ToString());
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
    }
}
