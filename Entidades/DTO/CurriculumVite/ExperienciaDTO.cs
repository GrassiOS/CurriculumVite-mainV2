using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class ExperienciaDTO
    {
        public int IdExperiencia { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El puesto es obligatorio")]
        public string? Puesto { get; set; }
        
        [Required(ErrorMessage = "La institución es obligatoria")]
        public string? Institucion { get; set; }
        
        public string? Descripcion { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime? FechaInicio { get; set; }
        
        public DateTime? FechaFin { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string PeriodoFormateado => 
            $"{FechaInicio?.ToString("MMM yyyy") ?? "N/A"} - {(FechaFin?.ToString("MMM yyyy") ?? "Actual")}";
        public bool EsActual => !FechaFin.HasValue;
        public int DuracionMeses
        {
            get
            {
                if (!FechaInicio.HasValue) return 0;
                var fechaFin = FechaFin ?? DateTime.Now;
                return (fechaFin.Year - FechaInicio.Value.Year) * 12 + 
                       (fechaFin.Month - FechaInicio.Value.Month);
            }
        }
        public string DuracionFormateada
        {
            get
            {
                var meses = DuracionMeses;
                var años = meses / 12;
                var mesesRestantes = meses % 12;
                
                if (años > 0 && mesesRestantes > 0)
                    return $"{años} año{(años > 1 ? "s" : "")} y {mesesRestantes} mes{(mesesRestantes > 1 ? "es" : "")}";
                else if (años > 0)
                    return $"{años} año{(años > 1 ? "s" : "")}";
                else
                    return $"{mesesRestantes} mes{(mesesRestantes > 1 ? "es" : "")}";
            }
        }
    }
}
