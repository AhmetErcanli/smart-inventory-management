using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Warehouses.Commands.UpdateWarehouse;

public class UpdateWarehouseCommand : IRequest<ApiResponse<WarehouseDto>>
{
    public UpdateWarehouseDto WarehouseDto { get; set; } = null!;
}

