using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Warehouses.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, ApiResponse<WarehouseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWarehouseCommandHandler> _logger;

    public UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateWarehouseCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<WarehouseDto>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.WarehouseDto.Id, cancellationToken);
            if (warehouse == null)
                return ApiResponse<WarehouseDto>.ErrorResponse("Warehouse not found.");

            _mapper.Map(request.WarehouseDto, warehouse);
            warehouse.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Warehouses.Update(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var warehouseDto = _mapper.Map<WarehouseDto>(warehouse);
            _logger.LogInformation("Warehouse updated: {WarehouseId} - {WarehouseName}", warehouse.Id, warehouse.WarehouseName);

            return ApiResponse<WarehouseDto>.SuccessResponse(warehouseDto, "Warehouse updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating warehouse");
            return ApiResponse<WarehouseDto>.ErrorResponse("An error occurred while updating the warehouse.");
        }
    }
}

