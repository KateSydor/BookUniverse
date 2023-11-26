﻿namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.CategoryRepository;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CategoryExists(string categoryName)
        {
            Category category = await _categoryRepository.Get(u => u.CategoryName == categoryName);
            if (category == null)
            {
                throw new Exception("Category does not exist.");
            }

            return category;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll().ToList();
        }

        public async Task AddCategory(string categoryName)
        {
            Category newCategory = new Category() { CategoryName = categoryName };
            await _categoryRepository.Create(newCategory);
        }

    }
}
