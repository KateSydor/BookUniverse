namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;
        private readonly RegistrationDto user;
        private NotifyWindow _notifyWindow = new NotifyWindow();
        public MainWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            IFolderService folderService,
            IBookFolderService bookFolderService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _folderService = folderService;
            _bookFolderService = bookFolderService;

            user = new RegistrationDto();
            this.DataContext = user;
        }

        private void Redirect_Signin_Button_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
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
                    HomeWindow homePage = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
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
