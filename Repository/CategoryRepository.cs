using API_Tutorial.Models;

namespace API_Tutorial.Repository
{
    public class CategoryRepository : ICategoryRepositories
    {
        private readonly MyDbContext context;

        public CategoryRepository(MyDbContext _context)
        {
            this.context = _context;
        }

        async Task<Category> ICategoryRepositories.Create(CategoryVM category)
        {
            Category c = new Category{
                Name=category.CategoryName
            };
            context.categories.Add(c);
            await context.SaveChangesAsync();
            return c;
        }

        async Task<Category> ICategoryRepositories.Delete(int id)
        {
            Category c = context.categories.FirstOrDefault(ca=>ca.Id==id);
            if(c==null) return null;
            context.categories.Remove(c);
            await context.SaveChangesAsync();
            return c;
        }

        List<Category> ICategoryRepositories.GetAll()
        {
            return context.categories.ToList();
        }

        Category ICategoryRepositories.GetCategory(int id)
        {
            return context.categories.FirstOrDefault(c=>c.Id==id);
        }

        async Task<Category> ICategoryRepositories.Update(int id, CategoryVM category)
        {
             Category c = context.categories.FirstOrDefault(ca=>ca.Id==id);
            if(c==null) return null;
            if(id!=category.Id) return null;
            c.Name=category.CategoryName;
            context.categories.Update(c);
            await context.SaveChangesAsync();
            return c;
        }
    }
}