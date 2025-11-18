using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApiResponse<PagedResultDto<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProductsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResultDto<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allProducts = await _unitOfWork.Products.GetAllAsync(cancellationToken);
            var productsList = allProducts.ToList();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                productsList = productsList.Where(p => 
                    p.Name.ToLower().Contains(searchTerm) || 
                    p.SKU.ToLower().Contains(searchTerm) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm))).ToList();
            }

            if (request.CategoryId.HasValue)
                productsList = productsList.Where(p => p.CategoryId == request.CategoryId.Value).ToList();

            if (request.SupplierId.HasValue)
                productsList = productsList.Where(p => p.SupplierId == request.SupplierId.Value).ToList();

            if (request.WarehouseId.HasValue)
                productsList = productsList.Where(p => p.WarehouseId == request.WarehouseId.Value).ToList();

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                productsList = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending 
                        ? productsList.OrderByDescending(p => p.Name).ToList()
                        : productsList.OrderBy(p => p.Name).ToList(),
                    "sku" => request.SortDescending
                        ? productsList.OrderByDescending(p => p.SKU).ToList()
                        : productsList.OrderBy(p => p.SKU).ToList(),
                    "quantity" => request.SortDescending
                        ? productsList.OrderByDescending(p => p.Quantity).ToList()
                        : productsList.OrderBy(p => p.Quantity).ToList(),
                    "createdat" => request.SortDescending
                        ? productsList.OrderByDescending(p => p.CreatedAt).ToList()
                        : productsList.OrderBy(p => p.CreatedAt).ToList(),
                    _ => productsList
                };
            }
            else
            {
                productsList = productsList.OrderByDescending(p => p.CreatedAt).ToList();
            }

            // Apply pagination
            var totalCount = productsList.Count;
            var pagedProducts = productsList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var productDtos = _mapper.Map<List<ProductDto>>(pagedProducts);

            // Load navigation properties
            foreach (var dto in productDtos)
            {
                var product = pagedProducts.First(p => p.Id == dto.Id);
                if (product.Category != null)
                    dto.CategoryName = product.Category.Name;
                if (product.Supplier != null)
                    dto.SupplierName = product.Supplier.SupplierName;
                if (product.Warehouse != null)
                    dto.WarehouseName = product.Warehouse.WarehouseName;
            }

            var result = new PagedResultDto<ProductDto>
            {
                Items = productDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return ApiResponse<PagedResultDto<ProductDto>>.SuccessResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return ApiResponse<PagedResultDto<ProductDto>>.ErrorResponse("An error occurred while retrieving products.");
        }
    }
}

