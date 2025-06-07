using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class ExperienciaProfile : Profile
    {
        public ExperienciaProfile()
        {
            CreateMap<ExperienciaDTO, E_Experiencia>().ReverseMap();
        }
    }
}
