namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
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
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;
        private readonly ISearchBook _searchBookService;
        private User currentUser;
        private int bookId;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        public BookWindow(IAuthenticationService authenticationService, IUserService userService, IBookService bookService, ICategoryService categoryService, IGoogleDriveService googleDriveRepository, ISearchBook searchBookService, int bookId, IFolderService folderService, IBookFolderService bookFolderService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _folderService = folderService;
            _bookFolderService = bookFolderService;
            _searchBookService = searchBookService;

            Loaded += Book_Loaded;
            Closed += Window_Closed;

            this.bookId = bookId;

            InitializeComponent();
            Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked += MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked += MenuControl_FavouriteBooksClicked;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            Menu.SearchBooksClicked -= MenuControl_SearchBooksClicked;
            Menu.FavouriteBooksClicked -= MenuControl_FavouriteBooksClicked;
        }

        private void MenuControl_SearchBooksClicked(object sender, EventArgs e)
        {
            BookSearch searchBooks = new BookSearch(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            searchBooks.Show();
            Close();
        }

        private void MenuControl_FavouriteBooksClicked(object sender, EventArgs e)
        {
            FavouriteBooksWindow listOfBooks = new FavouriteBooksWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            listOfBooks.Show();
            Close();
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
                signInPage.Show();
                Hide();
            }

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
        private void AddFolderClick(object sender, RoutedEventArgs e)
        {
            var book = this.DataContext as Book;

            FoldersWindow folderwindow = new FoldersWindow(_authenticationService,
            _userService,
            _bookService,
            _categoryService,
            _googleDriveRepository,
            _folderService, _bookFolderService, book);

            

            //folderwindow.Show();
            folderwindow.ShowDialog();
            

        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            signInPage.Show();
            Close();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            userAccount.Show();
            Close();

        }

        private void ReadButtonClick(object sender, RoutedEventArgs e)
        {
            ReadBook readBook = new ReadBook(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, bookId, _folderService, _bookFolderService);
            readBook.Show();
            Close();
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            homeWindow.Show();
            Close();
        }

        private async void FavButton_Click(object sender, RoutedEventArgs e)
        {
            UserBook existingUserBook = await _bookService.GetUserBook(currentUser.Id, bookId);
            if (existingUserBook == null)
            {
                UserBook newUserBook = CreateUserBookObject(isFav: true);
                await AddBookAndNotify(newUserBook);
            }
            else if (existingUserBook != null && existingUserBook.IsFavourite == false)
            {
                existingUserBook.IsFavourite = true;
                await _bookService.UpdateUserBook(existingUserBook);
                _notifyWindow.ShowNotification($"{UtilsConstants.SUCCESSFULLY_ADDED_BOOK} to favourites!");
            }
            else
            {
                _notifyWindow.ShowNotification($"{UtilsConstants.ALREADY_ADDED_BOOK} to favourites!");
            }
        }

        private async void AddToLibButton_Click(object sender, RoutedEventArgs e)
        {
            UserBook existingUserBook = await _bookService.GetUserBook(currentUser.Id, bookId);
            if (existingUserBook == null)
            {
                UserBook newUserBook = CreateUserBookObject();
                await AddBookAndNotify(newUserBook);
            }
            else
            {
                _notifyWindow.ShowNotification($"{UtilsConstants.ALREADY_ADDED_BOOK} to library!");
            }
        }

        private UserBook CreateUserBookObject(bool isFav = false)
        {
            return new UserBook()
            {
                UserId = currentUser.Id,
                BookId = bookId,
                IsFavourite = isFav,
            };
        }

        private async Task AddBookAndNotify(UserBook userbook)
        {
            try
            {
                await _bookService.AddUserBook(userbook);
                _notifyWindow.ShowNotification(UtilsConstants.SUCCESSFULLY_ADDED_BOOK);
            }
            catch
            {
                _notifyWindow.ShowNotification(UtilsConstants.ERROR);
            }
        }
    }
}