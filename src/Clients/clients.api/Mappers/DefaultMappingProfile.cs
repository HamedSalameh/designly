using AutoMapper;
using Clients.API.DTO;
using Clients.Domain.Entities;
using Designly.Shared.ValueObjects;

namespace Clients.API.Mappers
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>();
            
            CreateMap<AddressDto, Address>()
                .ConstructUsing(src =>
                new Address(src.City, src.Street, src.BuildingNumber, src.AddressLines));

            CreateMap<Address, AddressDto>()
                .ConvertUsing(src =>
                new AddressDto(src.City, src.Street, src.BuildingNumber, src.AddressLines));

            
            CreateMap<ContactDetailsDto, ContactDetails>()
                .ConvertUsing(src =>
                new ContactDetails(src.PrimaryPhoneNumber, src.SecondaryPhoneNumber, src.EmailAddress));

            CreateMap<ContactDetails, ContactDetailsDto>()
                .ConvertUsing(src =>
                new ContactDetailsDto(src.PrimaryPhoneNumber, src.SecondaryPhoneNumber, src.EmailAddress));
        }
    }
}
