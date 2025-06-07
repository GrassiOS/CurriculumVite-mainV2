using AutoMapper;
using Entidades.DTO.PlanesDeEstudio.Carreras;
using Entidades.Modelos.PlanesDeEstudio.Carreras;

namespace Entidades.PerfilesDTO.PlanesDeEstudio
{
  public class CarreraProfile : Profile
  {
    public CarreraProfile()
    {
      CreateMap<CarreraDTO, E_Carrera>().ReverseMap();      
    }
  }
}
