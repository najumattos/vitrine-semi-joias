using AutoMapper;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Common;

public class ConfigurationProfile : Profile
{
    public ConfigurationProfile()
    {
        CreateMap<ProductModel, ProductViewModel>().ReverseMap();

    }
}
