namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
    using BookUniverse.Client.CustomControls;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    /// <summary>
    /// Interaction logic for ListOfBooks.xaml.
    /// </summary>
    public partial class BookWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly ISearchBook _searchBookService;
        private User currentUser;
        private int bookId;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public BookWindow(IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            ISearchBook searchBookService, int bookId)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _searchBookService = searchBookService;

            Loaded += Book_Loaded;
            Closed += Window_Closed;

            this.bookId = bookId;

            InitializeComponent();
            Menu.AllBooksClicked += MenuControl_AllBooksClicked;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {
            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            listOfBooks.Show();
            Close();
        }

        private async void Book_Loaded(object sender, RoutedEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            SystemCommands.MaximizeWindow(this);
            this.DataContext = await _bookService.GetBook(bookId);
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
            Close();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            userAccount.Show();
            Close();

        }

        private void ReadButtonClick(object sender, RoutedEventArgs e)
        {
            ReadBook readBook = new ReadBook(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, bookId);
            readBook.Show();
            Close();
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            homeWindow.Show();
            Close();
        }

        private async void FavButton_Click(object sender, RoutedEventArgs e)
        {
            UserBook newUserBook = new UserBook()
            {
                UserId = currentUser.Id,
                BookId = bookId,
                IsFavourite = true,
            };

            try
            {
                await _bookService.AddUserBook(newUserBook);
                _notifyWindow.ShowNotification("Book is successfully added!");
            }
            catch
            {
                _notifyWindow.ShowNotification(UtilsConstants.ERROR);
            }
        }
    }
}