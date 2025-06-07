using Entidades.Modelos.PlanesDeEstudio.Carreras;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Modelos.PlanesDeEstudio.PlanEstudios
{
  public class E_PlanEstudio
  {
    public int IdPlanEstudio { get; set; }
    public string PlanEstudio { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int TotalCreditos { get; set; }    
    public int CreditosOptativos { get; set; }    
    public int CreditosObligatorios { get; set; }
    public string PerfilDeIngreso { get; set; }
    public string PerfilDeEgreso { get; set; }
    public string CampoOcupacional { get; set; }
    public string Comentarios { get; set; }

    public bool EstadoPlanEstudio { get; set; } = true;

    // Foreign key
    public int IdCarrera { get; set; }

    // Navigation properties
    public E_Carrera Carrera { get; set; }
    public ICollection<E_PlanEstudioMateria> PlanEstudioMaterias { get; set; }
  }
}
