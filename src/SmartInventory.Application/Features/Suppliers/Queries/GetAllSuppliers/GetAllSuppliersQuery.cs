using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersQuery : IRequest<ApiResponse<List<SupplierDto>>>
{
}

