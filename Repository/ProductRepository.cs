using API_Tutorial.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Tutorial.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext context;
        public static int Page_Size{get;set;} = 5;
        public ProductRepository(MyDbContext _context)
        {
            context = _context;
        }

        public async Task<ProductVM> Create(ProductVM p)
        {
            ProductModel product=new ProductModel{
                Name=p.ProductName,
                Price=p.Price
            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return new ProductVM{ProductName=product.Name,Price=product.Price};
        }

        public void Delete(int id)
        {
            ProductModel p = context.Products.FirstOrDefault(p=>p.Id==id);
            if(p!=null)
            {
                context.Products.Remove(p);
                context.SaveChanges();
            }
        }

        public async Task<ICollection<ProductModel>> GetAll(string? search,decimal? from,decimal? to,string sortby,int page = 1)
        {
            var listprouct=context.Products.Include(p=>p.category).AsQueryable();
            #region filter
            if(!string.IsNullOrEmpty(search))
            {
                listprouct.Where(p=>p.Name.Contains(search));
            }
            if(from.HasValue)
            {
                listprouct = listprouct.Where(p=>p.Price>=from);
            }
             if(to.HasValue)
            {
                listprouct = listprouct.Where(p=>p.Price<=to);
            }
            #endregion
            #region sorting
            //default name
            listprouct = listprouct.OrderBy(p=>p.Name);
            if(!string.IsNullOrEmpty(sortby))
            {
                switch(sortby)
                {
                    case "P_desc" : listprouct=listprouct.OrderByDescending(p=>p.Name); break; 
                    case "Price_asc" : listprouct=listprouct.OrderBy(p=>p.Price); break; 
                    case "Price_desc" : listprouct=listprouct.OrderByDescending(p=>p.Price); break; 
                }
                     
            }
            
            #endregion

            #region Paging
            // listprouct = listprouct.Skip((page-1)*Page_Size).Take(Page_Size);
            // return await Task.FromResult(listprouct.ToList());

            var resul=PaginatedList<ProductModel>.Create(listprouct,Page_Size,page);
            return await Task.FromResult(resul.ToList());

            #endregion

            
        }

        public async Task<ProductVM> GetProductById(int id)
        {
            var p=context.Products.FirstOrDefault(p=>p.Id==id);
            return await Task.FromResult(new ProductVM{ProductName = p.Name, Price = p.Price});
        }

        public async Task<ProductVM> Update(int id, ProductModel p)
        {
             var product=context.Products.FirstOrDefault(p=>p.Id==id);
            if(product!=null&&id==p.Id)
            {
                product.Name=p.Name;
                p.Price=p.Price;
                p.CategoryId=p.CategoryId;
                context.Products.Update(p);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(new ProductVM{ProductName=p.Name,Price=p.Price});
        }
    }
}