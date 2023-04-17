using API_Tutorial.Models;
namespace API_Tutorial.Repository
{
    public interface IProductRepository
    {
        Task<ICollection<ProductModel>> GetAll(string search,decimal? from,decimal? to,string sortby,int page);

        Task<ProductVM> GetProductById(int id);

        Task<ProductVM> Create(ProductVM p);

        Task<ProductVM> Update(int id,ProductModel p);

        void Delete(int id);
    }
}