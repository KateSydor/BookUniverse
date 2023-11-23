﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookUniverse.Client.CustomControls
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public static event EventHandler AllBooksClicked;
        public static event EventHandler SearchBooksClicked;

        public Menu()
        {
            InitializeComponent();
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


        private void ItemHome_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            AllBooksClicked?.Invoke(this, EventArgs.Empty);
            CloseWindow();
        }
        private void ItemHome_PreviewMouseDown2(object sender, MouseButtonEventArgs e)
        {
            SearchBooksClicked?.Invoke(this, EventArgs.Empty);
            CloseWindow();
        }
        private void CloseWindow()
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

    }
}
