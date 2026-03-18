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
    public async Task<ActionResult<Venta>> CrearVenta(Venta venta)
    {
        Venta ventaSaved = await _ventaService.CrearVenta(venta);

        return ventaSaved;
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
    public async Task<ActionResult<List<Venta>>> GetAll()
    {
        var ventas = await _ventaService.GetAll();

        return ventas;
    }
}