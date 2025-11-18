using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<ApiResponse<ProductDto>>
{
    public UpdateProductDto ProductDto { get; set; } = null!;
}

