using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if SKU already exists
            var existingProduct = await _unitOfWork.Products.FirstOrDefaultAsync(
                p => p.SKU == request.ProductDto.SKU, cancellationToken);

            if (existingProduct != null)
            {
                return ApiResponse<ProductDto>.ErrorResponse("Product with this SKU already exists.");
            }

            // Verify related entities exist
            var category = await _unitOfWork.Categories.GetByIdAsync(request.ProductDto.CategoryId, cancellationToken);
            if (category == null)
                return ApiResponse<ProductDto>.ErrorResponse("Category not found.");

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(request.ProductDto.SupplierId, cancellationToken);
            if (supplier == null)
                return ApiResponse<ProductDto>.ErrorResponse("Supplier not found.");

            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.ProductDto.WarehouseId, cancellationToken);
            if (warehouse == null)
                return ApiResponse<ProductDto>.ErrorResponse("Warehouse not found.");

            var product = _mapper.Map<Product>(request.ProductDto);
            await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = category.Name;
            productDto.SupplierName = supplier.SupplierName;
            productDto.WarehouseName = warehouse.WarehouseName;

            _logger.LogInformation("Product created: {ProductId} - {ProductName}", product.Id, product.Name);

            return ApiResponse<ProductDto>.SuccessResponse(productDto, "Product created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return ApiResponse<ProductDto>.ErrorResponse("An error occurred while creating the product.");
        }
    }
}

