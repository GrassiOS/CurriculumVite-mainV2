namespace Entidades.DTO.PlanesDeEstudio.Materias
{
  public class ListaMateriaDTO
  {
    public int IdMateria { get; set; }
    public string ClaveMateria { get; set; }
    public string NombreMateria { get; set; }
    public int HC { get; set; }
    public int HL { get; set; }
    public int HT { get; set; }
    public int HPC { get; set; }
    public int HCL { get; set; }
    public int HE { get; set; }
    public int CR { get; set; }
    public bool EstadoMateria { get; set; }

  }
}
