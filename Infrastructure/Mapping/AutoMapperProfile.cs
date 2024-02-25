using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository.Models;

namespace Infrastructure.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BondSnapshot, BondSnapshotDbModel>().ReverseMap();
        CreateMap<Bond, BondDbModel>().ReverseMap();
        CreateMap<BondValues, BondValuesDbModel>().ReverseMap();
    }
}
