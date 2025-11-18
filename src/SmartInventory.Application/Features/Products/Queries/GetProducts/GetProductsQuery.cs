using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<ApiResponse<PagedResultDto<ProductDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}

