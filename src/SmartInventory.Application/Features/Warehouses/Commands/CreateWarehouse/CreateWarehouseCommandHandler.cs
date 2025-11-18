using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, ApiResponse<WarehouseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWarehouseCommandHandler> _logger;

    public CreateWarehouseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateWarehouseCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = _mapper.Map<Warehouse>(request.WarehouseDto);
            await _unitOfWork.Warehouses.AddAsync(warehouse, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var warehouseDto = _mapper.Map<WarehouseDto>(warehouse);
            _logger.LogInformation("Warehouse created: {WarehouseId} - {WarehouseName}", warehouse.Id, warehouse.WarehouseName);

            return ApiResponse<WarehouseDto>.SuccessResponse(warehouseDto, "Warehouse created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating warehouse");
            return ApiResponse<WarehouseDto>.ErrorResponse("An error occurred while creating the warehouse.");
        }
    }
}

