using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class DocumentoProfile : Profile
    {
        public DocumentoProfile()
        {
            CreateMap<DocumentoDTO, E_Documento>().ReverseMap();
        }
    }
}
