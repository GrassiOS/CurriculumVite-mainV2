using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class EducacionProfile : Profile
    {
        public EducacionProfile()
        {
            CreateMap<EducacionDTO, E_Educacion>().ReverseMap();
        }
    }
}
