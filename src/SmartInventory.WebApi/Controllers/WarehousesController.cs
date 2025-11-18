using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Features.Warehouses.Commands.CreateWarehouse;
using SmartInventory.Application.Features.Warehouses.Commands.DeleteWarehouse;
using SmartInventory.Application.Features.Warehouses.Commands.UpdateWarehouse;
using SmartInventory.Application.Features.Warehouses.Queries.GetAllWarehouses;

namespace SmartInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<WarehouseDto>>>> GetWarehouses()
    {
        var query = new GetAllWarehousesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto)
    {
        var command = new CreateWarehouseCommand { WarehouseDto = warehouseDto };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetWarehouses), result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> UpdateWarehouse(int id, [FromBody] UpdateWarehouseDto warehouseDto)
    {
        if (id != warehouseDto.Id)
            return BadRequest(ApiResponse<WarehouseDto>.ErrorResponse("ID mismatch."));

        var command = new UpdateWarehouseCommand { WarehouseDto = warehouseDto };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteWarehouse(int id)
    {
        var command = new DeleteWarehouseCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

