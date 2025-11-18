using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Features.Auth.Commands.Login;
using SmartInventory.Application.Features.Auth.Commands.Register;

namespace SmartInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        var command = new LoginCommand { LoginDto = loginDto };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
    {
        var command = new RegisterCommand { RegisterDto = registerDto };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

