using BookUniverse.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Constants.UtilsConstants;
using MaterialDesignThemes.Wpf;

namespace BookUniverse.Client
{
    /// <summary>
    /// Interaction logic for FoldersWindow.xaml
    /// </summary>
    public partial class FoldersWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IFolderService _folderService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IBookFolderService _bookFolderService;
        private User currentUser;
        private NotifyWindow _notifyWindow = new NotifyWindow();

        private List<object> foldersList;
        private Book currentBook;


        public FoldersWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            IFolderService folderService,
            IBookFolderService bookFolderService,
            Book book)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _folderService = folderService;
            _bookFolderService = bookFolderService;
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
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
            catch (Exception ex) { _notifyWindow.ShowNotification($"Error: {ex.Message}"); }

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
                NewFolderWindow newFolderWindow = new NewFolderWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService, currentBook.Id);
                newFolderWindow.Show();
                Close();
            }
            catch (Exception ex) { _notifyWindow.ShowNotification($"Error:\n{ex.Message}"); }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}