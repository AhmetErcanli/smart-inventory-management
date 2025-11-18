using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Warehouses.Queries.GetAllWarehouses;

public class GetAllWarehousesQueryHandler : IRequestHandler<GetAllWarehousesQuery, ApiResponse<List<WarehouseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllWarehousesQueryHandler> _logger;

    public GetAllWarehousesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllWarehousesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<List<WarehouseDto>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouses = await _unitOfWork.Warehouses.GetAllAsync(cancellationToken);
            var warehouseDtos = _mapper.Map<List<WarehouseDto>>(warehouses);
            return ApiResponse<List<WarehouseDto>>.SuccessResponse(warehouseDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving warehouses");
            return ApiResponse<List<WarehouseDto>>.ErrorResponse("An error occurred while retrieving warehouses.");
        }
    }
}

