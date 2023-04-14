using API_Tutorial.Models;
using API_Tutorial.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API_Tutorial.Controllers;
[ApiController]
[Route("[Controller]")]
public class CategoryController:ControllerBase
{
    private readonly ICategoryRepositories repo;

    public  CategoryController(ICategoryRepositories _repo)
    {
        repo = _repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        
        return Ok(repo.GetAll());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryVM categoryVM)
    {
        Category c= await repo.Create(categoryVM);
        return Ok(c);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id,CategoryVM category)
    {
        Category c= await repo.Update(id,category);
        if(c!=null)
            return Ok(c);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Category c= await repo.Delete(id);
        if(c!=null)
            return Ok(c);
        return NotFound();
    }
}