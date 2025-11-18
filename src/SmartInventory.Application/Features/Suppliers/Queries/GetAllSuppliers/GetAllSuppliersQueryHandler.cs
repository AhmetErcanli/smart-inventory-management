using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, ApiResponse<List<SupplierDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSuppliersQueryHandler> _logger;

    public GetAllSuppliersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllSuppliersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<List<SupplierDto>>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var suppliers = await _unitOfWork.Suppliers.GetAllAsync(cancellationToken);
            var supplierDtos = _mapper.Map<List<SupplierDto>>(suppliers);
            return ApiResponse<List<SupplierDto>>.SuccessResponse(supplierDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving suppliers");
            return ApiResponse<List<SupplierDto>>.ErrorResponse("An error occurred while retrieving suppliers.");
        }
    }
}

