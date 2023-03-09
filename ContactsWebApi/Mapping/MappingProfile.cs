using AutoMapper;
using ContactsWebApi.Dtos;
using ContactsWebApi.Models.Entities;

namespace ContactsWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactDto, Contact>();
        }
    }
}
