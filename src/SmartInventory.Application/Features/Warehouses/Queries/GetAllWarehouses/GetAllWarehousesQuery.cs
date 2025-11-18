using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Warehouses.Queries.GetAllWarehouses;

public class GetAllWarehousesQuery : IRequest<ApiResponse<List<WarehouseDto>>>
{
}

