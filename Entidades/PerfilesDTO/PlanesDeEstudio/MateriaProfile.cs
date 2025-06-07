using AutoMapper;
using Entidades.DTO.PlanesDeEstudio.Materias;
using Entidades.Modelos.PlanesDeEstudio.Materias;

namespace Entidades.PerfilesDTO.PlanesDeEstudio
{
  public class MateriaProfile : Profile
  {

    public MateriaProfile()
    {
      CreateMap<MateriaDTO, E_Materia>().ReverseMap(); 
      CreateMap<ListaMateriaDTO, E_Materia>().ReverseMap();     
    }
  }
}
