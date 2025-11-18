using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Warehouses.Commands.DeleteWarehouse;

public class DeleteWarehouseCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }
}

