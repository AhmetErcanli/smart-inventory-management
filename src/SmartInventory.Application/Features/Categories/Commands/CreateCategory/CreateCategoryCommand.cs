using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<ApiResponse<CategoryDto>>
{
    public CreateCategoryDto CategoryDto { get; set; } = null!;
}

