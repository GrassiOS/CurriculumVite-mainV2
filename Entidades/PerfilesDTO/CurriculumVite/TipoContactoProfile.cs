using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class TipoContactoProfile : Profile
    {
        public TipoContactoProfile()
        {
            CreateMap<TipoContactoDTO, E_TipoContacto>().ReverseMap();
        }
    }
}
