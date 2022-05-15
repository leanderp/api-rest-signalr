using api.Context;
using api.Entities;

using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDBContext _context;

        public CategoryRepository(AppDBContext context)
        {
            _context = context;
        }

        public bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(p => p.Id == id);
        }

        public async Task CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetAllCategory()
        {
            var categories = await _context.Categories.Where(p => p.Enabled == true).ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategoryById(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            return category;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                _context.Entry(category).State.Equals(EntityState.Modified);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) when (!CategoryExists(category.Id))
            {
                return false;
            }
        }
    }
}
