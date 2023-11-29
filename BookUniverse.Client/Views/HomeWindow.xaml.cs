namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.Client.CustomControls;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for HomeWindow.xaml.
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveService;
        private readonly ISearchBook _searchBookService;
        private User currentUser;
        private List<object> bookList;
        private int currentPage = 1;
        private int booksPerPage = 13;

        public HomeWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveService,
            ISearchBook searchBookService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveService = googleDriveService;
            _searchBookService = searchBookService;

            Loaded += HomeWindow_Loaded;
            Closed += Window_Closed;

            bookList = new List<object> { };
            List<Book> tempBookList = _bookService.GetAllBooks();
            if (tempBookList.Count != 0)
            {
                for (int i = 0; i < tempBookList.Count; i++)
                {
                    bookList.Add(new { Number = tempBookList[i].Id, tempBookList[i].Title, tempBookList[i].Author, tempBookList[i].NumberOfPages, tempBookList[i].Rating });
                }
            }


            this.DataContext = currentUser;
            InitializeComponent();
            dataGrid.ItemsSource = displayedBooks;
            CustomControls.Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            CustomControls.Menu.SearchBooksClicked += MenuControl_SearchBooksClicked;

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


        private void Window_Closed(object sender, EventArgs e)
        {
            CustomControls.Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            CustomControls.Menu.SearchBooksClicked -= MenuControl_SearchBooksClicked;
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {
            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            listOfBooks.Show();
            Close();
        }

        private void MenuControl_SearchBooksClicked(object sender, EventArgs e)
        {
            BookSearch listOfBooks = new BookSearch(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            listOfBooks.Show();
            Close();
        }

        private async void HomeWindow_Loaded(object sender, RoutedEventArgs e)
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
                }
                else
                {
                    throw new Exception(UtilsConstants.FILE_ERROR);
                }
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
                signInPage.Show();
                Hide();
            }
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

                BookWindow bookWindow = new BookWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService, numberProperty);
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
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            signInPage.Show();
            Close();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveService, _searchBookService);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
            Close();
        }
    }
}
