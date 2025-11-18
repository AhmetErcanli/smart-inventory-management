using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }
}

