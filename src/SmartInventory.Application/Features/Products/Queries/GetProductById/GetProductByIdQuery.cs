using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ApiResponse<ProductDto>>
{
    public int Id { get; set; }
}

