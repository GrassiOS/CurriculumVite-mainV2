using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class TesisDirigidaProfile : Profile
    {
        public TesisDirigidaProfile()
        {
            CreateMap<TesisDirigidaDTO, E_TesisDirigida>().ReverseMap();
        }
    }
}
