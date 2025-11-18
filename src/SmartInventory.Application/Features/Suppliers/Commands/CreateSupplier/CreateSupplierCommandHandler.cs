using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, ApiResponse<SupplierDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSupplierCommandHandler> _logger;

    public CreateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateSupplierCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var supplier = _mapper.Map<Supplier>(request.SupplierDto);
            await _unitOfWork.Suppliers.AddAsync(supplier, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            _logger.LogInformation("Supplier created: {SupplierId} - {SupplierName}", supplier.Id, supplier.SupplierName);

            return ApiResponse<SupplierDto>.SuccessResponse(supplierDto, "Supplier created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating supplier");
            return ApiResponse<SupplierDto>.ErrorResponse("An error occurred while creating the supplier.");
        }
    }
}

