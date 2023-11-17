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
    public partial class Book : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private User currentUser;

        public Book(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;

            Loaded += Book_Loaded;

            this.DataContext = currentUser;
            InitializeComponent();
        }

        private async void Book_Loaded(object sender, RoutedEventArgs e)
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