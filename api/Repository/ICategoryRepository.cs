using api.Entities;

namespace api.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategory();
        Task<Category?> GetCategoryById(Guid id);
        Task CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task DeleteCategory(Guid id);
        bool CategoryExists(Guid id);
    }
}
