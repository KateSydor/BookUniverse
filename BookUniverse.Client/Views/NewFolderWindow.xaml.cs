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
using System.IO;
using BookUniverse.DAL.Constants.UtilsConstants;
using BookUniverse.DAL.Entities;
using BookUniverse.BLL.Interfaces;

namespace BookUniverse.Client
{
    public partial class NewFolderWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IFolderService _folderService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IBookFolderService _bookFolderService;

        private NotifyWindow notifyWindow = new NotifyWindow();
        private User currentUser;
        private int currentBookId;

        public NewFolderWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            IFolderService folderService,
            IBookFolderService bookFolderService,
            int book_id
            )
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _folderService = folderService;
            _bookFolderService = bookFolderService;

            currentBookId = book_id;

            Loaded += ListOfFolder_Loaded;
            this.DataContext = currentUser;

            InitializeComponent();
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
                Hide();
            }
        }

        private async void Add_folderClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var folderInstance = new Folder
                {
                    FolderName = NameOfFolder.Text,
                    UserId = currentUser.Id
                };
                await _folderService.AddNewFolder(folderInstance);

                var lastFolder = await _folderService.GetLastFolder(); ///ПЕРЕРОБИТИ 

                var folderInstance2 = new BookFolder
                {
                    BookId = currentBookId,
                    FolderId = lastFolder.Id
                };
                await _bookFolderService.AddInFolder(folderInstance2);

                Hide();
                notifyWindow.ShowNotification("New folder was created \nand a book was added to it");
            }
            catch (Exception ex)
            {
                notifyWindow.ShowNotification($"Error: {ex.Message}");
            }


        }
    }
}