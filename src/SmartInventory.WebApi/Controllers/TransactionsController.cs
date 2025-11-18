using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Features.Transactions.Commands.CreateTransaction;
using SmartInventory.Application.Features.Transactions.Queries.GetAllTransactions;
using System.Security.Claims;

namespace SmartInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<PagedResultDto<InventoryTransactionDto>>>> GetTransactions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? productId = null)
    {
        var query = new GetAllTransactionsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            ProductId = productId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<InventoryTransactionDto>>> CreateTransaction([FromBody] CreateTransactionDto transactionDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse<InventoryTransactionDto>.ErrorResponse("Invalid user."));

        var command = new CreateTransactionCommand
        {
            TransactionDto = transactionDto,
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetTransactions), result);
    }
}

