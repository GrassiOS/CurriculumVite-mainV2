using AutoMapper;
using Entidades.DTO.PlanesDeEstudio.PlanEstudios;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;

namespace Entidades.PerfilesDTO.PlanesDeEstudio
{
  internal class PlanEstudiosProfile: Profile
  {
    public PlanEstudiosProfile()
    {
      CreateMap<E_PlanEstudio, PlanEstudioDTO>().ReverseMap();
    }
  }
}
