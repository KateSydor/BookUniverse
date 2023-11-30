namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.DTOValidators.UserValidators;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using FluentValidation.Results;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;
        private readonly ISearchBook _searchBookService;
        private readonly RegistrationDto user;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public MainWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            ISearchBook searchBookService,
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
            _searchBookService = searchBookService;

            user = new RegistrationDto();
            this.DataContext = user;
        }

        private void Redirect_Signin_Button_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            this.Visibility = Visibility.Hidden;
            signInWindow.Show();
        }

        private async void Signup_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistrationDtoValidator validator = new RegistrationDtoValidator();
                ValidationResult validationResult = validator.Validate(user);

                if (validationResult.IsValid)
                {
                    await _authenticationService.Register(user);
                    if (_authenticationService.IsLoggedIn())
                    {
                        HomeWindow homePage = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
                        homePage.Show();
                        Close();
                    }
                }
                else
                {
                    HomeWindow homePage = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
                    homePage.Show();
                    Close();
                    _notifyWindow.ShowNotification(UtilsConstants.INPUT_VALID_DATA);
                }
            }
            catch (ArgumentException argEx)
            {
                _notifyWindow.ShowNotification("Error: " + argEx.Message);
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
