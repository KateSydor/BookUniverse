namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for ListOfBooks.xaml.
    /// </summary>
    public partial class ListOfBooks : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private User currentUser;
        private List<Book> bookList;
        private int currentPage = 1;
        private int booksPerPage = 13;

        public ListOfBooks(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;

            Loaded += ListOfBooks_Loaded;

            this.DataContext = currentUser;
            bookList = _bookService.GetAllBooks();

            InitializeComponent();
            dataGrid.ItemsSource = displayedBooks;
        }

        private List<Book> displayedBooks
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

        private async void  ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage * booksPerPage > bookList.Count)
            {
                return;
            }

            currentPage++;
            DisplayBooks();
        }

        private async void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1)
            {
                return;
            }

            currentPage--;
            DisplayBooks();

        }

        private async void ListOfBooks_Loaded(object sender, RoutedEventArgs e)
        {
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
                signInPage.Show();
                Hide();
            }
        }

        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement source && source.DataContext != null)
            {
                var clickedItem = source.DataContext;

                BookWindow bookWindow = new BookWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
                this.Hide();
                bookWindow.Show();
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
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
            signInPage.Show();
            Hide();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
        }
    }
}