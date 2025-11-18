using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, ApiResponse<SupplierDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSupplierCommandHandler> _logger;

    public UpdateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateSupplierCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(request.SupplierDto.Id, cancellationToken);
            if (supplier == null)
                return ApiResponse<SupplierDto>.ErrorResponse("Supplier not found.");

            _mapper.Map(request.SupplierDto, supplier);
            supplier.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            _logger.LogInformation("Supplier updated: {SupplierId} - {SupplierName}", supplier.Id, supplier.SupplierName);

            return ApiResponse<SupplierDto>.SuccessResponse(supplierDto, "Supplier updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating supplier");
            return ApiResponse<SupplierDto>.ErrorResponse("An error occurred while updating the supplier.");
        }
    }
}

