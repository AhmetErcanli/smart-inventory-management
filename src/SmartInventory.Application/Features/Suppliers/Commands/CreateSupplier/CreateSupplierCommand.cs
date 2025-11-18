using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommand : IRequest<ApiResponse<SupplierDto>>
{
    public CreateSupplierDto SupplierDto { get; set; } = null!;
}

