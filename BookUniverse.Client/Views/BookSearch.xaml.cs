namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
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
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;

        private User currentUser;
        private NotifyWindow _notifyWindow = new NotifyWindow();
        public BookSearch(
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


            Loaded += SearchOfBooks_Loaded;
            Closed += Window_Closed;

            this.DataContext = currentUser;

            InitializeComponent();
            CustomControls.Menu.AllBooksClicked += MenuControl_AllBooksClicked;
            CustomControls.Menu.FavouriteBooksClicked += MenuControl_FavouriteBooksClicked;
            GetCategories();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            CustomControls.Menu.AllBooksClicked -= MenuControl_AllBooksClicked;
            CustomControls.Menu.FavouriteBooksClicked -= MenuControl_FavouriteBooksClicked;
        }

        private void MenuControl_AllBooksClicked(object sender, EventArgs e)
        {
            ListOfBooks listOfBooks = new ListOfBooks(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private void MenuControl_FavouriteBooksClicked(object sender, EventArgs e)
        {
            FavouriteBooksWindow listOfBooks = new FavouriteBooksWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            listOfBooks.Show();
            Close();
        }

        private async void SearchOfBooks_Loaded(object sender, RoutedEventArgs e)
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
                signInPage.Show();
                Close();
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
                        resultListBox.Items.Add($"{volume.VolumeInfo.Title} by {string.Join(", ", volume.VolumeInfo.Authors ?? Array.Empty<string>())}");
                    }
                }
                catch
                {
                    _notifyWindow.ShowNotification("No book found");
                    resultListBox.Items.Clear();
                }
            }
        }

        private void ResultListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;

            if (listBox.SelectedItem != null)
            {
                string selectedText = listBox.SelectedItem.ToString();
                Clipboard.SetText(selectedText);
                copyPopup.IsOpen = true;
                Task.Delay(2000).ContinueWith(_ => Dispatcher.Invoke(() => copyPopup.IsOpen = false));
            }
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            homeWindow.Show();
            Close();
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            userAccount.Show();
            Close();
        }
        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
            signInPage.Show();
            Close();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
    }
}
