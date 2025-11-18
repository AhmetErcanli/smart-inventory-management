using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommand : IRequest<ApiResponse<SupplierDto>>
{
    public UpdateSupplierDto SupplierDto { get; set; } = null!;
}

