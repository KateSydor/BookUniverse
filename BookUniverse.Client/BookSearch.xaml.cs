﻿using BookUniverse.BLL.Interfaces;
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

using System.IO;
using BookUniverse.DAL.Constants.UtilsConstants;
using BookUniverse.DAL.Entities;

using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Constants.UtilsConstants;

namespace BookUniverse.Client
{
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

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    var volumes = await _searchBookService.SearchAsync(searchText);

                    List<string> searchResults = new List<string>();

                    foreach (var volume in volumes.Items)
                    {
                        string result = $"{volume.VolumeInfo.Title} by {string.Join(", ", volume.VolumeInfo.Authors)}";
                        searchResults.Add(result);
                    }

                    resultListBox.ItemsSource = searchResults;
                }
                catch (Exception ex)
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
