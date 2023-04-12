using API_Tutorial.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Tutorial.Controllers;

    [ApiController]
    [Route("Controller")]
    public class ProductController : ControllerBase
    {
        private readonly MyDbContext _context;
        public ProductController(MyDbContext context)
        {
            _context=context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products=_context.Products.ToList();
            return Ok(products);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var product=_context.Products.FirstOrDefault(p=>p.Id==id);
            if(product==null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM p)
        {
            ProductModel product=new ProductModel{
                Name=p.ProductName,
                Price=p.Price
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok(product);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var p= _context.Products.FirstOrDefault(p=>p.Id==id);
            if(p!=null)
            {
                _context.Products.Remove(p);
                _context.SaveChanges();
                return Ok(p);
            }
            else
                return NotFound();
            
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id,ProductModel p)
        {
            var product = _context.Products.FirstOrDefault(p1=>p1.Id==id);
            if(product==null)
                return NotFound();
            if(id!=p.Id)
                return BadRequest();
            product.Name=p.Name;
            product.Price=p.Price;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }
    }
