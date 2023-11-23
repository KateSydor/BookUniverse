namespace BookUniverse.BLL.Interfaces
{
    using BookUniverse.DAL.Entities;

    public interface ICategoryService
    {
        Task<Category> CategoryExists(string categoryName);

        List<Category> GetAllCategories();

        void AddCategory(string categoryName);
    }
}
