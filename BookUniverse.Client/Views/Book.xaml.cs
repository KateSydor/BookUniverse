﻿namespace BookUniverse.Client
{
    using System;
    using System.IO;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Services;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    /// <summary>
    /// Interaction logic for ListOfBooks.xaml.
    /// </summary>
    public partial class BookWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IGoogleDriveService _googleDriveRepository;
        private readonly IFolderService _folderService;
        private readonly IBookFolderService _bookFolderService;
        private User currentUser;
        private int bookId;

        public BookWindow(IAuthenticationService authenticationService, IUserService userService, IBookService bookService, ICategoryService categoryService, IGoogleDriveService googleDriveRepository, int bookId, IFolderService folderService, IBookFolderService bookFolderService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _bookService= bookService;
            _categoryService = categoryService;
            _googleDriveRepository = googleDriveRepository;
            _folderService = folderService;
            _bookFolderService = bookFolderService;

            Loaded += Book_Loaded;
            this.bookId = bookId;
            
            InitializeComponent();
        }

        private async void Book_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await _bookService.GetBook(bookId);
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
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
                signInPage.Show();
                Hide();
            }

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }
        private void AddFolderClick(object sender, RoutedEventArgs e)
        {
            var book = this.DataContext as Book;

            FoldersWindow folderwindow = new FoldersWindow(_authenticationService,
            _userService,
            _bookService,
            _categoryService,
            _googleDriveRepository,
            _folderService, _bookFolderService, book);

            

            //folderwindow.Show();
            folderwindow.ShowDialog();
            

        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.Logout();
            SignInWindow signInPage = new SignInWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
            signInPage.Show();
            Hide();
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
            this.Visibility = Visibility.Hidden;
            userAccount.Show();
        }

        private void ReadButtonClick(object sender, RoutedEventArgs e)
        {
            ReadBook readBook = new ReadBook(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, bookId, _folderService, _bookFolderService);
            this.Visibility = Visibility.Hidden;
            readBook.Show();
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService, _bookService, _categoryService, _googleDriveRepository, _folderService, _bookFolderService);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
        }
    }
}