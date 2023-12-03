namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for ListOfBooks.xaml.
    /// </summary>
    public partial class FavouriteBooksWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly ISearchBook _searchBookService;
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;
        private User currentUser;
        private List<object> bookList;
        private int currentPage = 1;
        private int booksPerPage = 13;

        public FavouriteBooksWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            ISearchBook searchBookService,
            IFolderService folderService,
            IBookFolderService bookFolderService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _searchBookService = searchBookService;
            _folderService = folderService;
            _bookFolderService = bookFolderService;

            Loaded += FavouriteBooksWindow_Loaded;
            Closed += Window_Closed;

            this.DataContext = currentUser;
            bookList = new List<object> { };

            InitializeComponent();
            CustomControls.Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            CustomControls.Menu.SearchBooksClicked += MenuControl_SearchBooksClicked;
            GetCategories();
            GetFolders();
        }

        private void GetCategories()
        {
            try
            {
                Menu_Control.Menu_Categories.ItemsSource = _categoryService.GetAllCategories().Select(category => category.CategoryName).ToList();
            }
            catch
            {
                Menu_Control.Menu_Categories.ItemsSource = new List<string>() { "No categories found" };
            }
        }

        private void GetFolders()
        {
            try
            {
                Menu_Control.Menu_Folders.ItemsSource = _folderService.GetAllFolders().Select(category => category.FolderName).ToList();
            }
            catch
            {
                Menu_Control.Menu_Folders.ItemsSource = new List<string>() { "No folders found" };
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CustomControls.Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            CustomControls.Menu.SearchBooksClicked -= MenuControl_SearchBooksClicked;
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {
            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private void MenuControl_SearchBooksClicked(object sender, EventArgs e)
        {
            BookSearch listOfBooks = new BookSearch(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private List<object> displayedBooks
        {
            get
            {
                int startIndex = (currentPage - 1) * booksPerPage;
                return bookList.Skip(startIndex).Take(booksPerPage).ToList();
            }
        }

        private void DisplayBooks()
        {
            dataGrid.ItemsSource = displayedBooks;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage * booksPerPage > bookList.Count)
            {
                return;
            }

            currentPage++;
            DisplayBooks();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1)
            {
                return;
            }

            currentPage--;
            DisplayBooks();
        }

        private async void FavouriteBooksWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            SystemCommands.MaximizeWindow(this);
            try
            {
                string[] lines = File.ReadAllLines(UtilsConstants.FILE_PATH);
                if (lines.Length >= 2)
                {
                    int userId = int.Parse(lines[0]);
                    string userEmail = lines[1];

                    currentUser = await _userService.GetUser(userEmail);
                    username.Text = currentUser.Username;

                    GetBooks();
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
                Close();
            }
        }

        private void GetBooks()
        {
            List<Book> tempBookList = _bookService.GetUserFavouriteBooks(currentUser.Email);
            if (tempBookList.Count != 0)
            {
                for (int i = 0; i < tempBookList.Count; i++)
                {
                    bookList.Add(new { Number = tempBookList[i].Id, tempBookList[i].Title, tempBookList[i].Author, tempBookList[i].NumberOfPages, tempBookList[i].Rating });
                }
            }

            dataGrid.ItemsSource = displayedBooks;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Number")
            {
                e.Cancel = true;
            }
        }

        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement source && source.DataContext != null)
            {
                var clickedItem = source.DataContext;

                var numberProperty = (int)clickedItem.GetType().GetProperty("Number")?.GetValue(clickedItem, null);

                BookWindow bookWindow = new BookWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, numberProperty, _folderService, _bookFolderService);
                bookWindow.Show();
                Close();
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

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            homeWindow.Show();
            Close();
        }
    }
}