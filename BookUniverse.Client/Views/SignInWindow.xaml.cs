namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;

    /// <summary>
    /// Interaction logic for SignInWindow.xaml.
    /// </summary>
    public partial class SignInWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly LoginDto user;

        public SignInWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;

            user = new LoginDto();
            this.DataContext = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow signUpWindow = new MainWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
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
                    HomeWindow homePage = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
                    homePage.Show();
                    Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, UtilsConstants.ERROR);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
    }
}
