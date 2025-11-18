using AutoMapper;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, ProductDto>();

        // Category mappings
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();

        // Supplier mappings
        CreateMap<CreateSupplierDto, Supplier>();
        CreateMap<UpdateSupplierDto, Supplier>();
        CreateMap<Supplier, SupplierDto>();

        // Warehouse mappings
        CreateMap<CreateWarehouseDto, Warehouse>();
        CreateMap<UpdateWarehouseDto, Warehouse>();
        CreateMap<Warehouse, WarehouseDto>();

        // Transaction mappings
        CreateMap<InventoryTransaction, InventoryTransactionDto>();
    }
}

