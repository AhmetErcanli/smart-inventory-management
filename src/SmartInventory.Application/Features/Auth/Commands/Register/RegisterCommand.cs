using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<ApiResponse<AuthResponseDto>>
{
    public RegisterDto RegisterDto { get; set; } = null!;
}

