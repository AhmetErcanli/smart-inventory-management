using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    public DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return ApiResponse<bool>.ErrorResponse("Category not found.");

            // Check if category has products
            var hasProducts = await _unitOfWork.Products.ExistsAsync(p => p.CategoryId == request.Id, cancellationToken);
            if (hasProducts)
                return ApiResponse<bool>.ErrorResponse("Cannot delete category with associated products.");

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category deleted: {CategoryId} - {CategoryName}", category.Id, category.Name);

            return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category");
            return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the category.");
        }
    }
}

