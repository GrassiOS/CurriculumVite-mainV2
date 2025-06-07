using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class ProyectoProfile : Profile
    {
        public ProyectoProfile()
        {
            CreateMap<ProyectoDTO, E_Proyecto>().ReverseMap();
        }
    }
}
