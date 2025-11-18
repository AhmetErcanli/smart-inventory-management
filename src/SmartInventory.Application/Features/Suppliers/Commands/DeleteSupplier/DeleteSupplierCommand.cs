using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }
}

