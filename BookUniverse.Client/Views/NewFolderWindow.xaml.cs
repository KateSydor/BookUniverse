namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;

    public partial class NewFolderWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookManagementService _bookService;
        private readonly IFolderService _folderService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IBookFolderService _bookFolderService;
        private readonly ISearchBook _searchBookService;

        private NotifyWindow notifyWindow = new NotifyWindow();
        private User currentUser;
        private int currentBookId;

        public NewFolderWindow(
            IAuthenticationService authenticationService,
            IUserService userService,
            IBookManagementService bookService,
            ICategoryService categoryService,
            IGoogleDriveService googleDriveRepository,
            IFolderService folderService,
            IBookFolderService bookFolderService,
            ISearchBook searchBookService,
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
            _searchBookService = searchBookService;

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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _searchBookService, _folderService, _bookFolderService);
                signInPage.Show();
                Close();
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
                var createdFolder = await _folderService.AddNewFolder(folderInstance, currentUser.Id);

                var bookFolderInstance = new BookFolder
                {
                    BookId = currentBookId,
                    FolderId = createdFolder.Id
                };
                await _bookFolderService.AddInFolder(bookFolderInstance);

                Close();
                notifyWindow.ShowNotification("New folder was created \nand a book was added to it");
            }
            catch (Exception ex)
            {
                notifyWindow.ShowNotification($"Error: {ex.Message}");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close without saving?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}