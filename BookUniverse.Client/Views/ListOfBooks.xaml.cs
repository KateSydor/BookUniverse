namespace BookUniverse.Client
{
    using BookUniverse.Client;
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ListOfBooks.xaml.
    /// </summary>
    public partial class ListOfBooks : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private User currentUser;
        private List<object> bookList;  
        private int currentPage = 1;    
        private int booksPerPage = 13;


        public ListOfBooks(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;

            Loaded += ListOfBooks_Loaded;

            this.DataContext = currentUser;
            bookList = new List<object>
        {
            new { Number = "1", Title = "Cat", Author = "KateSydor" },
            new { Number = "2", Title = "Cat2", Author = "KateSydor" },
            new { Number = "3", Title = "Cat3", Author = "KateSydor" },
            new { Number = "4", Title = "Cat", Author = "KateSydor" },
            new { Number = "5", Title = "Cat2", Author = "KateSydor" },
            new { Number = "6", Title = "Cat3", Author = "KateSydor" },
            new { Number = "7", Title = "Cat", Author = "KateSydor" },
            new { Number = "8", Title = "Cat2", Author = "KateSydor" },
            new { Number = "9", Title = "Cat3", Author = "KateSydor" },
            new { Number = "10", Title = "Cat", Author = "KateSydor" },
            new { Number = "11", Title = "Cat2", Author = "KateSydor" },
            new { Number = "12", Title = "Cat3", Author = "KateSydor" },
             new { Number = "13", Title = "Cat", Author = "KateSydor" },
            new { Number = "14", Title = "Cat2", Author = "KateSydor" },
            new { Number = "15", Title = "Cat3", Author = "KateSydor" },
            new { Number = "16", Title = "Cat", Author = "KateSydor" },
            new { Number = "17", Title = "Cat2", Author = "KateSydor" },
            new { Number = "18", Title = "Cat3", Author = "KateSydor" },
            new { Number = "19", Title = "Cat", Author = "KateSydor" },
            new { Number = "20", Title = "Cat2", Author = "KateSydor" },
            new { Number = "21", Title = "Cat3", Author = "KateSydor" },
            new { Number = "22", Title = "Cat", Author = "KateSydor" },
            new { Number = "23", Title = "Cat2", Author = "KateSydor" },
            new { Number = "24", Title = "Cat3", Author = "KateSydor" },
        };
            

            InitializeComponent();
            dataGrid.ItemsSource = displayedBooks;

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


        private async void  ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage*booksPerPage> bookList.Count) { return; }
            currentPage++;
            DisplayBooks();

        }

        private async void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1) { return; }
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService);
                signInPage.Show();
                Hide();
            }
        }

        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement source && source.DataContext != null)
            {
                var clickedItem = source.DataContext;

                Book bookWindow = new Book(_authenticationService, _userService);
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
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService);
            signInPage.Show();
            Hide();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
        }
    }
}