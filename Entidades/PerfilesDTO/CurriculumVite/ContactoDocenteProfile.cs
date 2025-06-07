using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class ContactoDocenteProfile : Profile
    {
        public ContactoDocenteProfile()
        {
            CreateMap<ContactoDocenteDTO, E_ContactoDocente>()
                .ForMember(dest => dest.IdTpoContacto, opt => opt.MapFrom(src => src.IdTipoContacto))
                .ReverseMap()
                .ForMember(dest => dest.IdTipoContacto, opt => opt.MapFrom(src => src.IdTpoContacto))
                .ForMember(dest => dest.NombreTipoContacto, opt => opt.MapFrom(src => src.TipoContacto.Nombre));
        }
    }
}
