using ApiApartamentos.DTOs;
using AutoMapper;
using DataAccessLayer.Models;

namespace ApiApartamentos.Mappings
{
    public class ApartamentoProfile : Profile
    {
        public ApartamentoProfile()
        {
            CreateMap<Apartamento, ApartamentoDTO>(); // Para el Cliente Web
            CreateMap<Apartamento, ApartamentoFullDTO>(); // Para WinForms
        }
    }
}
