using InventoryService.Data;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentaController : ControllerBase
{
    private readonly InventoryDbContext _dbContext;

    public VentaController(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> RealizarVenta([FromBody] Venta venta)
    {
        _dbContext.Ventas.Add(venta);
        await _dbContext.SaveChangesAsync();

        return Ok(venta);
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        var ventas = await _dbContext.Ventas.Include(v => v.Detalles).ToListAsync();

        return Ok(ventas);
    }
}