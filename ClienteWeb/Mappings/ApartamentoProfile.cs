using ApiApartamentos.DTOs;
using AutoMapper;
using ClienteWeb.Models.ViewModels;

namespace ClienteWeb.Mappings
{
    public class ApartamentoProfile : Profile
    {
        public ApartamentoProfile()
        {
            CreateMap<ApartamentoDTO, ApartamentoViewModel>();
        }
    }
}
