namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.CategoryRepository;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggingService _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILoggingService logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<Category> CategoryExists(string categoryName)
        {
            Category category = await _categoryRepository.Get(u => u.CategoryName == categoryName);
            if (category == null)
            {
                string errMsg = "Category does not exist";
                _logger.LogError(null, errMsg);
                throw new Exception(errMsg);
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
