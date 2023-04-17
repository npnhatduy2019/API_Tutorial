using API_Tutorial.Models;

namespace API_Tutorial.Repository
{
    public interface ICategoryRepositories
    {
        public Task<IEnumerable<Category>> GetAll();

        public Category GetCategory(int id);

        public Task<Category> Create(CategoryVM category);

        public Task<Category> Update(int id, CategoryVM category);

        public Task<Category> Delete(int id);
    }
}