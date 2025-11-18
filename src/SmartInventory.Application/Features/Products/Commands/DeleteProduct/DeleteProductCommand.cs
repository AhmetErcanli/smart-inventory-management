using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }
}

