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
        private readonly RegistrationDto user;

        public MainWindow(IAuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;

            user = new RegistrationDto();
            this.DataContext = user;
        }

        private void Redirect_Signin_Button_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow(_authenticationService);
            this.Visibility = Visibility.Hidden;
            signInWindow.Show();
        }

        private async void Signup_Button_Click(object sender, RoutedEventArgs e)
        {
            string pass = password.Password.Trim();
            string repeatPass = repeatPassword.Password.Trim();

            try
            {
                await _authenticationService.Register(user);
                if (_authenticationService.IsLoggedIn())
                {
                    SignInWindow homePage = new(_authenticationService);
                    homePage.Show();
                    Hide();
                }
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message, "Error");
            }
            catch
            {
                MessageBox.Show("Not valid data", "Error");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();

        }
    }
}
