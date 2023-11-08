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
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Constants.UtilsConstants;
using System.IO;


namespace BookUniverse.Client
{
    /// <summary>
    /// Interaction logic for UserAccount.xaml
    /// </summary>
    public partial class UserAccount : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private User currentUser;

        public UserAccount(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;

            Loaded += UserAccount_Loaded;
            this.DataContext = currentUser;

            InitializeComponent();
        }

        private async void UserAccount_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines(UtilsConstants.FILE_PATH);
                if (lines.Length >= 2)
                {
                    int userId = int.Parse(lines[0]);
                    string userEmail = lines[1];

                    currentUser = await _userService.GetUser(userEmail);
                    Username_on_top.Text = currentUser.Username;
                    edit_username.Text = currentUser.Username;
                    edit_email.Text = currentUser.Email;

                }
                else
                {
                    throw new Exception("File does not contain necessary information.");
                }
            }
            catch
            {
                SignInWindow signInPage = new SignInWindow(_authenticationService, _userService);
                signInPage.Show();
                Hide();
            }
        }


        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.Shutdown();

        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(_authenticationService, _userService);
            this.Visibility = Visibility.Hidden;
            homeWindow.Show();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        { }
        }
}
