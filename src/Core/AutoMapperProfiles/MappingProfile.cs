namespace Core.AutoMapperProfiles
{
    using AutoMapper;
    using Infrastructure.DTOs.Company;
    using Infrastructure.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyCreateDto, Company>();
            CreateMap<CompanyUpdateDto, Company>();
            CreateMap<Company, CompanyReadDto>();
        }
    }
}

