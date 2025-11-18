using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProductByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResponse("Product not found.");

            var productDto = _mapper.Map<ProductDto>(product);
            
            // Load navigation properties if needed
            if (product.Category != null)
                productDto.CategoryName = product.Category.Name;
            if (product.Supplier != null)
                productDto.SupplierName = product.Supplier.SupplierName;
            if (product.Warehouse != null)
                productDto.WarehouseName = product.Warehouse.WarehouseName;

            return ApiResponse<ProductDto>.SuccessResponse(productDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product");
            return ApiResponse<ProductDto>.ErrorResponse("An error occurred while retrieving the product.");
        }
    }
}

