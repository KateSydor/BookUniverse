namespace BookUniverse.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for FoldersWindow.xaml
    /// </summary>
    public partial class FoldersWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly IFolderService _folderService;
        private readonly ICategoryService _categoryService;
        private readonly ISearchBook _searchBookService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IBookFolderService _bookFolderService;
        private User currentUser;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        private List<object> foldersList;
        private Book currentBook;

        public FoldersWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            IFolderService folderService,
            IBookFolderService bookFolderService,
            ISearchBook searchBookService,
            Book book)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _folderService = folderService;
            _bookFolderService = bookFolderService;
            _searchBookService = searchBookService;
            currentBook = book;

            Loaded += ListOfFolder_Loaded;

            this.DataContext = currentUser;
            foldersList = new List<object> { };
            List<Folder> tempFolderList = _folderService.GetAllFolders();
            if (tempFolderList.Count != 0)
            {
                for (int i = 0; i < tempFolderList.Count; i++)
                {
                    foldersList.Add(new { Number = tempFolderList[i].Id, tempFolderList[i].FolderName });
                }
            }

            InitializeComponent();
            dataGrid.ItemsSource = foldersList;
        }

        private async void ListOfFolder_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines(UtilsConstants.FILE_PATH);
                if (lines.Length >= 2)
                {
                    int userId = int.Parse(lines[0]);
                    string userEmail = lines[1];

                    currentUser = await _userService.GetUser(userEmail);
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

        private async void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try {
                if (e.OriginalSource is FrameworkElement source && source.DataContext != null)
                {
                    var clickedItem = source.DataContext;

                    var numberProperty = (int)clickedItem.GetType().GetProperty("Number")?.GetValue(clickedItem, null);
                    var nameFolder = (string)clickedItem.GetType().GetProperty("FolderName")?.GetValue(clickedItem, null);

                    var bookFolderInstance = new BookFolder
                    {
                        BookId = currentBook.Id,
                        FolderId = numberProperty
                    };

                    await _bookFolderService.AddInFolder(bookFolderInstance);
                    this.Visibility = Visibility.Hidden;
                    _notifyWindow.ShowNotification($"Book was added in folder \n -{nameFolder}-");
                }
            }
            catch (Exception ex)
            {
                _notifyWindow.ShowNotification($"Error: {ex.Message}");
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

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewFolderWindow newFolderWindow = new NewFolderWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService,_searchBookService, currentBook.Id);
                newFolderWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                _notifyWindow.ShowNotification($"Error:\n{ex.Message}");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}