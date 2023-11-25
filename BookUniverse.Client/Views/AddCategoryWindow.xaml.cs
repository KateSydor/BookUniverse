﻿namespace BookUniverse.Client
{
    using System;
    using System.Windows;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;

    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private readonly ICategoryService _categoryService;
        private readonly Action<string> _updateCategoriesCallback;
        private Category category;

        public AddCategoryWindow(ICategoryService categoryService, Action<string> updateCategoriesCallback)
        {
            _categoryService = categoryService;
            _updateCategoriesCallback = updateCategoriesCallback;
            category = new Category();
            this.DataContext = category;

            InitializeComponent();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newCategoryName = CategoryNameTextBox.Text;
            await _categoryService.AddCategory(newCategoryName);
            _updateCategoriesCallback.Invoke(newCategoryName);
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close without saving?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
