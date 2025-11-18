using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommand : IRequest<ApiResponse<WarehouseDto>>
{
    public CreateWarehouseDto WarehouseDto { get; set; } = null!;
}

