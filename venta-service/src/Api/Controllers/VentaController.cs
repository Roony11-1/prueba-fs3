using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> CrearVenta(Venta venta)
    {
        var userId = User.FindFirst("sub")?.Value;

        if (userId == null)
            return Unauthorized();

        venta.UsuarioId = userId;

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
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirst("sub")?.Value;

        if (userId == null)
            return Unauthorized();

        var ventas = await _ventaService.GetAllByUsuarioId(userId);
        return Ok(ventas);
    }
}