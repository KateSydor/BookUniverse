namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.DTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Repositories.GoogleDriveRepository;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IGoogleDriveRepository _googleDriveRepository;
        private readonly RegistrationDto user;

        public MainWindow(IAuthenticationService authenticationService, IUserService userService, IGoogleDriveRepository googleDriveRepository)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userService = userService;
            _googleDriveRepository = googleDriveRepository;

            user = new RegistrationDto();
            this.DataContext = user;
        }

        private void Redirect_Signin_Button_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow(_authenticationService, _userService, _googleDriveRepository);
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
                    HomeWindow homePage = new HomeWindow(_authenticationService, _userService, _googleDriveRepository);
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
