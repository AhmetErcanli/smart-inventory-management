using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductDto.Id, cancellationToken);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResponse("Product not found.");

            // Check SKU uniqueness if changed
            if (product.SKU != request.ProductDto.SKU)
            {
                var existingProduct = await _unitOfWork.Products.FirstOrDefaultAsync(
                    p => p.SKU == request.ProductDto.SKU && p.Id != request.ProductDto.Id, cancellationToken);
                if (existingProduct != null)
                    return ApiResponse<ProductDto>.ErrorResponse("Product with this SKU already exists.");
            }

            // Verify related entities
            var category = await _unitOfWork.Categories.GetByIdAsync(request.ProductDto.CategoryId, cancellationToken);
            if (category == null)
                return ApiResponse<ProductDto>.ErrorResponse("Category not found.");

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(request.ProductDto.SupplierId, cancellationToken);
            if (supplier == null)
                return ApiResponse<ProductDto>.ErrorResponse("Supplier not found.");

            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.ProductDto.WarehouseId, cancellationToken);
            if (warehouse == null)
                return ApiResponse<ProductDto>.ErrorResponse("Warehouse not found.");

            _mapper.Map(request.ProductDto, product);
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = category.Name;
            productDto.SupplierName = supplier.SupplierName;
            productDto.WarehouseName = warehouse.WarehouseName;

            _logger.LogInformation("Product updated: {ProductId} - {ProductName}", product.Id, product.Name);

            return ApiResponse<ProductDto>.SuccessResponse(productDto, "Product updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product");
            return ApiResponse<ProductDto>.ErrorResponse("An error occurred while updating the product.");
        }
    }
}

