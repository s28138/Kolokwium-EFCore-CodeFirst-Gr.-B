using KolokwiumAPBDCodeFirst.DTOs;

using KolokwiumAPBDCodeFirst.Services;

using Microsoft.AspNetCore.Mvc;

namespace KolokwiumAPBDCodeFirst.Controllers;
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    public OrdersController(IOrderService service)
    {
        _service = service;
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await _service.GetOrder(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> ProcessOrder(
        UpdateOrderDto dto)
    {
        var result = await _service.ProcessOrder(dto);
        if (!result)
            return NotFound();
        return Ok("Order processed");
    }
}