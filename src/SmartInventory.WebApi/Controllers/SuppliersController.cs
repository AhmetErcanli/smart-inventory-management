using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Features.Suppliers.Commands.CreateSupplier;
using SmartInventory.Application.Features.Suppliers.Commands.DeleteSupplier;
using SmartInventory.Application.Features.Suppliers.Commands.UpdateSupplier;
using SmartInventory.Application.Features.Suppliers.Queries.GetAllSuppliers;

namespace SmartInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SupplierDto>>>> GetSuppliers()
    {
        var query = new GetAllSuppliersQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> CreateSupplier([FromBody] CreateSupplierDto supplierDto)
    {
        var command = new CreateSupplierCommand { SupplierDto = supplierDto };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetSuppliers), result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> UpdateSupplier(int id, [FromBody] UpdateSupplierDto supplierDto)
    {
        if (id != supplierDto.Id)
            return BadRequest(ApiResponse<SupplierDto>.ErrorResponse("ID mismatch."));

        var command = new UpdateSupplierCommand { SupplierDto = supplierDto };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSupplier(int id)
    {
        var command = new DeleteSupplierCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

