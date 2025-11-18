using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryDto.Id, cancellationToken);
            if (category == null)
                return ApiResponse<CategoryDto>.ErrorResponse("Category not found.");

            _mapper.Map(request.CategoryDto, category);
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var categoryDto = _mapper.Map<CategoryDto>(category);
            _logger.LogInformation("Category updated: {CategoryId} - {CategoryName}", category.Id, category.Name);

            return ApiResponse<CategoryDto>.SuccessResponse(categoryDto, "Category updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category");
            return ApiResponse<CategoryDto>.ErrorResponse("An error occurred while updating the category.");
        }
    }
}

