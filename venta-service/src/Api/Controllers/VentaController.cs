using Microsoft.AspNetCore.Mvc;
using VentaService.Application;
using VentaService.Domain;

namespace VentaService.Api.Controllers;

[ApiController]
[Route("api/venta")]
public class VentaController(IVentaService ventaService) : ControllerBase
{
    private readonly IVentaService _ventaService = ventaService;

    [HttpPost]
    public async Task<IActionResult> CrearVenta(Venta venta)
    {
        var result = await _ventaService.CrearVenta(venta);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Venta>> GetById(int id)
    {
        var venta = await _ventaService.GetById(id);

        if (venta == null)
            return NotFound();

        return venta;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ventas = await _ventaService.GetAll();
        return Ok(ventas);
    }
}