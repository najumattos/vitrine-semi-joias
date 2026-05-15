using AutoMapper;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Common;

public class ConfigurationProfile : Profile
{
    public ConfigurationProfile()
    {
        CreateMap<ProductModel, ProductDto>().ReverseMap();
        CreateMap<ProductDto, ProductViewModel>().ReverseMap();

        CreateMap<UserModel, LoginDto>().ReverseMap();
        CreateMap<LoginDto, LoginViewModel>().ReverseMap();
        CreateMap<UserModel, AuthUserDto>().ReverseMap();

    }
}
