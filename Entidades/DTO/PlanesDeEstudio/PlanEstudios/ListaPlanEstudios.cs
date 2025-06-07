namespace Entidades.DTO.PlanesDeEstudio.PlanEstudios
{
  public class ListaPlanEstudiosDTO
  {
    public int? IdPlanEstudio { get; set; }
    public string NombreCarrera { get; set; }
    public string ClaveCarrera { get; set; }    
    public string PlanEstudio { get; set; }
    public bool EstadoPlanEstudio { get; set; } = true;
  }
}
