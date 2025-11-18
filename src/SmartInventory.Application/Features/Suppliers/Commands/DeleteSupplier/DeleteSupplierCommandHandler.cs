using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSupplierCommandHandler> _logger;

    public DeleteSupplierCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteSupplierCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(request.Id, cancellationToken);
            if (supplier == null)
                return ApiResponse<bool>.ErrorResponse("Supplier not found.");

            var hasProducts = await _unitOfWork.Products.ExistsAsync(p => p.SupplierId == request.Id, cancellationToken);
            if (hasProducts)
                return ApiResponse<bool>.ErrorResponse("Cannot delete supplier with associated products.");

            _unitOfWork.Suppliers.Remove(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Supplier deleted: {SupplierId} - {SupplierName}", supplier.Id, supplier.SupplierName);

            return ApiResponse<bool>.SuccessResponse(true, "Supplier deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting supplier");
            return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the supplier.");
        }
    }
}

