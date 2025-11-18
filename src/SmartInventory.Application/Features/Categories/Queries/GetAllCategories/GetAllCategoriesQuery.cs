using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<ApiResponse<List<CategoryDto>>>
{
}

