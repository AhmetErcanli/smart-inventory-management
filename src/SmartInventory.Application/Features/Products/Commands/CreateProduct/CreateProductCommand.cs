using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<ApiResponse<ProductDto>>
{
    public CreateProductDto ProductDto { get; set; } = null!;
}

