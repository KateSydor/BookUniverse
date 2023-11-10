namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.DTOs;
    using BookUniverse.BLL.Interfaces;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly RegistrationDto user;
        private NotifyWindow _notifyWindow = new NotifyWindow();
        public MainWindow(IAuthenticationService authenticationService, IUserService userService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userService = userService;

            user = new RegistrationDto();
            this.DataContext = user;
        }

        private void Redirect_Signin_Button_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow(_authenticationService, _userService);
            this.Visibility = Visibility.Hidden;
            signInWindow.Show();
        }

        private async void Signup_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _authenticationService.Register(user);
                if (_authenticationService.IsLoggedIn())
                {
                    HomeWindow homePage = new HomeWindow(_authenticationService, _userService);
                    homePage.Show();
                    Hide();
                }
            }
            catch (ArgumentException argEx)
            {
                _notifyWindow.ShowNotification("Error: " +  argEx.Message.ToString());
            }
            catch
            {
                _notifyWindow.ShowNotification("Error: Not valid data");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
    }
}
