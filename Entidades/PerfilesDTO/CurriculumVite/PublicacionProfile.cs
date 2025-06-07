using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class PublicacionProfile : Profile
    {
        public PublicacionProfile()
        {
            CreateMap<PublicacionDTO, E_Publicacion>().ReverseMap();
        }
    }
}
