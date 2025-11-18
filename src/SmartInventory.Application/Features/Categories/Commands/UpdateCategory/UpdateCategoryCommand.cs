using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<ApiResponse<CategoryDto>>
{
    public UpdateCategoryDto CategoryDto { get; set; } = null!;
}

