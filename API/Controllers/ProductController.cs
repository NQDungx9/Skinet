using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly StoreContext _context;
    public ProductController(StoreContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProduct()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {

        return await _context.Products.FindAsync(id);
    }
}
