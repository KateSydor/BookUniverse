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
        private readonly LoginDto user;

        public SignInWindow(IAuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;

            user = new LoginDto();
            this.DataContext = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow signUpWindow = new MainWindow(_authenticationService);
            this.Visibility = Visibility.Hidden;
            signUpWindow.Show();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string pass = password.Password.Trim();

            try
            {
                await _authenticationService.Login(username.Text, pass);
                if (_authenticationService.IsLoggedIn())
                {
                    MainWindow homePage = new(_authenticationService);
                    homePage.Show();
                    Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();

        }
    }
}
