using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects.Property;
using Entities.DataTransferObjects.Landlord;
using Entities.DataTransferObjects.Tenant;
using Entities.DataTransferObjects.Address;
using Entities.DataTransferObjects.Invoice;
using Entities.DataTransferObjects.Photo;
using Entities.DataTransferObjects.Rate;
using Entities.DataTransferObjects.Rent;
using Entities.DataTransferObjects.State;

namespace SimpleFakeRent
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Landlord, LandlordDto>();
            CreateMap<Landlord, LandlordWithPropertiesDto>();
            CreateMap<LandlordForCreationDto, Landlord>();
            CreateMap<LandlordForUpdateDto, Landlord>();
            CreateMap<Landlord, LandlordWithAddressDto>();
            
            CreateMap<Property, PropertyDto>();
            CreateMap<Property, PropertyLimitedDto>();
            CreateMap<Property, PropertyWithTenantsDto>();
            CreateMap<Property, PropertyWithAddressDto>();
            CreateMap<Property, PropertyWithAddressAndRateDto>();
            CreateMap<Property, PropertyWithAddressLimitedDto>();
            CreateMap<Property, PropertyWithRateDto>();
            CreateMap<Property, PropertyWithRateAndAddressDto>();
            CreateMap<Property, PropertyForGetLandlordDto>();
            CreateMap<PropertyForCreationDto, Property>();
            CreateMap<PropertyForUpdateDto, Property>();
            CreateMap<Property, PropertyWithRentsDto>();
            CreateMap<Property, PropertyForAppDto>();

            CreateMap<Tenant, TenantDto>();
            CreateMap<Tenant, TenantWithEmailDto>();
            CreateMap<Tenant, TenantWithAddressDto>();
            CreateMap<TenantForCreationDto, Tenant>();
            CreateMap<TenantForUpdateDto, Tenant>();

            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressWithLandlordDto>(); 
            CreateMap<Address, AddressWithPropertyDto>();
            CreateMap<Address, AddressWithTenantDto>();
            CreateMap<AddressForCreationDto, Address>();
            CreateMap<AddressForUpdateDto, Address>();

            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Invoice, InvoiceForUpdateDto>();
            CreateMap<InvoiceForCreationDto, Invoice>();
            CreateMap<InvoiceForUpdateDto, Invoice>();
            CreateMap<Invoice, InvoiceWithRentDto>();
            CreateMap<Invoice, InvoiceWithRentStateTenantPropertyWithAddressLimitedDto>();
            CreateMap<Invoice, InvoiceDocumentDto>();
            CreateMap<Invoice, InvoiceWithStateDto>();

            CreateMap<Photo, PhotoDto>();
            CreateMap<Photo, PhotoColdWaterDto>();
            CreateMap<Photo, PhotoHotWaterDto>();
            CreateMap<Photo, PhotoGasDto>();
            CreateMap<Photo, PhotoEnergyDto>();
            CreateMap<Photo, PhotoHeatDto>(); 
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<PhotoForUpdateDto, Photo>();

            CreateMap<Rate, RateDto>();
            CreateMap<RateForCreationDto, Rate>();
            CreateMap<RateForUpdateDto, Rate>();
            CreateMap<Rate, RateWithPropertyDto>();
            CreateMap<Rate, RateWithPropertyAndAddressDto>();

            CreateMap<Rent, RentDto>();
            CreateMap<Rent, RentWithPropertyDto>();
            CreateMap<Rent, RentWithStatesDto>();
            CreateMap<Rent, RentWithInvoicesDto>();
            CreateMap<Rent, RentWithLandlordDto>();
            CreateMap<Rent, RentWithTenantDto>();
            CreateMap<RentForCreationDto, Rent>();
            CreateMap<RentForUpdateDto, Rent>();
            CreateMap<Rent, RentWithInvoiceAndStateDto>();
            CreateMap<Rent, RentForAppDto>();
            CreateMap<Rent, RentWithPropertyForAppDto>();
            CreateMap<Rent, RentWithPropertyAndAddressDto>();
            CreateMap<Rent, RentWithPropertyLimitedDto>();
            CreateMap<Rent, RentWithTenantAndPropertyWithAddressLimitedDto>();
            CreateMap<Rent, RentWithTenantAndPropertyWithAddressDto>();
            CreateMap<Rent, RentWithPropertyTenantAndAddressDto>();
            CreateMap<Rent, RentWithPropertyAndAddressLimitedDto>();
            CreateMap<Rent, RentWithPropertyAndAddressAndRateLimitedDto>();
            CreateMap<Rent, RentWithStateTenantPropertyWithAddressLimitedDto>();

            CreateMap<State, StateDto>();
            CreateMap<State, StateWithInvoiceDto>();
            CreateMap<State, StatesWithInvoiceDto>();
            CreateMap<State, StateWithPhotoDto>();
            CreateMap<State, StatesWithPhotoDto>();
            CreateMap<State, StateWithRentDto>();
            CreateMap<StateForCreationDto, State>();
            CreateMap<StateForUpdateDto, State>();
            CreateMap<State, StateWithInvoiceAndPhotoDto>();
        }
    }
}
