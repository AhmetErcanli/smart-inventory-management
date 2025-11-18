using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Features.Dashboard.Queries.GetDashboardData;

namespace SmartInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> GetDashboardData()
    {
        var query = new GetDashboardDataQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

