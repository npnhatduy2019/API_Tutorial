using API_Tutorial.Models;
using API_Tutorial.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API_Tutorial.Controllers;
[ApiController]
[Route("[Controller]")]
public class ProductRepoController:ControllerBase
{
    private readonly IProductRepository repo;

    public ProductRepoController(IProductRepository _repo)
    {
        repo = _repo;
    }

    // [HttpGet("{search?}")]
    // public async Task<IActionResult> GetAll(string search = "")
    // {
    //     return Ok(await repo.GetAll(search));
    // }
    [HttpGet]
    public async Task<IActionResult> GetAll(string? search, decimal? from, decimal? to, string? sortby,int page = 1)
    {
        return Ok(await repo.GetAll(search,from,to,sortby,page));
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductVM p)
    {
        var product= await repo.Create(p);
        return Ok(product);
    }
}
