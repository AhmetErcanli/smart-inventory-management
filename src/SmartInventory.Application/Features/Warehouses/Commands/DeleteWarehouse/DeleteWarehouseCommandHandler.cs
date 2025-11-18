using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Warehouses.Commands.DeleteWarehouse;

public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteWarehouseCommandHandler> _logger;

    public DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteWarehouseCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.Id, cancellationToken);
            if (warehouse == null)
                return ApiResponse<bool>.ErrorResponse("Warehouse not found.");

            var hasProducts = await _unitOfWork.Products.ExistsAsync(p => p.WarehouseId == request.Id, cancellationToken);
            if (hasProducts)
                return ApiResponse<bool>.ErrorResponse("Cannot delete warehouse with associated products.");

            _unitOfWork.Warehouses.Remove(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Warehouse deleted: {WarehouseId} - {WarehouseName}", warehouse.Id, warehouse.WarehouseName);

            return ApiResponse<bool>.SuccessResponse(true, "Warehouse deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting warehouse");
            return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the warehouse.");
        }
    }
}

