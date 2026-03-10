using InventoryService.Data;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{
    private readonly InventoryDbContext _dbContext;

    public ProductoController(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var productos = await _dbContext.Productos.ToListAsync();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _dbContext.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Producto producto)
    {
        _dbContext.Productos.Add(producto);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }
}