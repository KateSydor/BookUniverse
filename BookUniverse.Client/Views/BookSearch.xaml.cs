namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for BookSearch.xaml
    /// </summary>
    public partial class BookSearch : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly ISearchBook _searchBookService;
        private User currentUser;
        private NotifyWindow _notifyWindow = new NotifyWindow();
        public BookSearch(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            ISearchBook searchBookService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _searchBookService = searchBookService;


            Loaded += SearchOfBooks_Loaded;
            this.DataContext = currentUser;

            InitializeComponent();
        }

        private async void SearchOfBooks_Loaded(object sender, RoutedEventArgs e)
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
                signInPage.Show();
                Hide();
            }
        }
        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text;
            string searchQuery = $"\"{searchText}\"";

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    var volumes = await _searchBookService.SearchAsync(searchQuery);

                    resultListBox.Items.Clear();

                    foreach (var volume in volumes.Items)
                    {
                        resultListBox.Items.Add($"{volume.VolumeInfo.Title} by {string.Join(", ", volume.VolumeInfo.Authors)}");
                    }
                }
                catch
                {
                    _notifyWindow.ShowNotification("No book found");
                    resultListBox.ItemsSource = "";
                }
            }
        }





        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }
        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService);
            signInPage.Show();
            Hide();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
    }
}
