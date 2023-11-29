namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    /// <summary>
    /// Interaction logic for ListOfBooks.xaml.
    /// </summary>
    public partial class ReadBook : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly ISearchBook _searchBookService;

        private NotifyWindow _notifyWindow = new NotifyWindow();

        private User currentUser;
        private int bookId;

        private Book currBook;

        public ReadBook(IAuthenticationService authenticationService, IUserService userService, IBookManagementService bookService, ICategoryService categoryService, IGoogleDriveService googleDriveRepository, ISearchBook searchBookService, int bookId)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _searchBookService = searchBookService;

            this.bookId = bookId;

            Loaded += Book_Loaded;

            InitializeComponent();
        }

        private async void Book_Loaded(object sender, RoutedEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            SystemCommands.MaximizeWindow(this);
            await GetCurrentBook();
            try
            {
                string[] lines = File.ReadAllLines(UtilsConstants.FILE_PATH);
                if (lines.Length >= 2)
                {
                    int userId = int.Parse(lines[0]);
                    string userEmail = lines[1];

                    currentUser = await _userService.GetUser(userEmail);
                    username.Text = currentUser.Username;
                }
                else
                {
                    throw new Exception(UtilsConstants.FILE_ERROR);
                }
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
                signInPage.Show();
                Hide();
            }
        }

        private async Task GetCurrentBook()
        {
            try
            {
                currBook = await _bookService.GetBook(bookId);
                myweb.Source = new Uri(currBook.Path, UriKind.RelativeOrAbsolute);
            }
            catch
            {
                _notifyWindow.ShowNotification("Something went wrong. Please try again later");
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
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            signInPage.Show();
            Hide();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
        }
    }
}