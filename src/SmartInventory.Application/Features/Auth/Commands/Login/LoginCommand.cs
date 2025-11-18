using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<ApiResponse<AuthResponseDto>>
{
    public LoginDto LoginDto { get; set; } = null!;
}

