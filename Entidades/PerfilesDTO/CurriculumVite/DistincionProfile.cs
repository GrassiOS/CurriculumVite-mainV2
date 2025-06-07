using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class DistincionProfile : Profile
    {
        public DistincionProfile()
        {
            CreateMap<DistincionDTO, E_Distincion>().ReverseMap();
        }
    }
}
