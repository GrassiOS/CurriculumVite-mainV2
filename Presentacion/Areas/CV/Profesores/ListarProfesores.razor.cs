using Entidades.Modelos.CurriculumVite;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Presentacion.Areas.CV.Profesores
{
    partial class ListarProfesores : ComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private IEnumerable<E_Docente> LstProfesores { get; set; } = [];
        private IEnumerable<E_Docente> LstProfesoresOriginal { get; set; } = [];
        private string CriterioBusqueda { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            // Static data since database is not configured
            LstProfesoresOriginal = GenerarDatosEstaticos();
            LstProfesores = LstProfesoresOriginal;
            await base.OnInitializedAsync();
        }

        private async Task BuscarProfesor()
        {
            if (string.IsNullOrWhiteSpace(CriterioBusqueda))
            {
                LstProfesores = LstProfesoresOriginal;
            }
            else
            {
                LstProfesores = LstProfesoresOriginal.Where(p => 
                    p.NombreDocente.Contains(CriterioBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    (p.ApellidoPaterno?.Contains(CriterioBusqueda, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.ApellidoMaterno?.Contains(CriterioBusqueda, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    p.Email.Contains(CriterioBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    p.Telefono.Contains(CriterioBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    (p.Especialidad?.Contains(CriterioBusqueda, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }
            await Task.CompletedTask;
        }

        private async Task BorrarProfesor(int idProfesor)
        {
            try
            {
                bool confirmacion = await JSRuntime.InvokeAsync<bool>("confirm", "¿Está seguro de eliminar este profesor?");

                if (confirmacion)
                {
                    var profesorAEliminar = LstProfesoresOriginal.FirstOrDefault(p => p.IdDocente == idProfesor);
                    if (profesorAEliminar != null)
                    {
                        LstProfesoresOriginal = LstProfesoresOriginal.Where(p => p.IdDocente != idProfesor).ToList();
                        LstProfesores = LstProfesoresOriginal;
                        
                        await JSRuntime.InvokeVoidAsync("alert", "Profesor eliminado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Error al eliminar el profesor: {ex.Message}");
            }
        }

        private IEnumerable<E_Docente> GenerarDatosEstaticos()
        {
            return new List<E_Docente>
            {
                new E_Docente
                {
                    IdDocente = 1,
                    NombreDocente = "María Elena",
                    ApellidoPaterno = "González",
                    ApellidoMaterno = "Rodríguez",
                    Email = "maria.gonzalez@uabc.edu.mx",
                    Telefono = "(664) 123-4567",
                    Cedula = "1234567890",
                    Especialidad = "Ingeniería de Software",
                    FechaRegistro = DateTime.Now.AddYears(-2),
                    EstadoDocenteBool = true
                },
                new E_Docente
                {
                    IdDocente = 2,
                    NombreDocente = "Juan Carlos",
                    ApellidoPaterno = "Martínez",
                    ApellidoMaterno = "López",
                    Email = "juan.martinez@uabc.edu.mx",
                    Telefono = "(664) 234-5678",
                    Cedula = "2345678901",
                    Especialidad = "Bases de Datos",
                    FechaRegistro = DateTime.Now.AddYears(-3),
                    EstadoDocenteBool = true
                },
                new E_Docente
                {
                    IdDocente = 3,
                    NombreDocente = "Ana Sofía",
                    ApellidoPaterno = "Hernández",
                    ApellidoMaterno = "García",
                    Email = "ana.hernandez@uabc.edu.mx",
                    Telefono = "(664) 345-6789",
                    Cedula = "3456789012",
                    Especialidad = "Inteligencia Artificial",
                    FechaRegistro = DateTime.Now.AddYears(-1),
                    EstadoDocenteBool = true
                },
                new E_Docente
                {
                    IdDocente = 4,
                    NombreDocente = "Roberto",
                    ApellidoPaterno = "Sánchez",
                    ApellidoMaterno = "Morales",
                    Email = "roberto.sanchez@uabc.edu.mx",
                    Telefono = "(664) 456-7890",
                    Cedula = "4567890123",
                    Especialidad = "Redes y Comunicaciones",
                    FechaRegistro = DateTime.Now.AddYears(-4),
                    EstadoDocenteBool = false
                },
                new E_Docente
                {
                    IdDocente = 5,
                    NombreDocente = "Carmen",
                    ApellidoPaterno = "Jiménez",
                    ApellidoMaterno = "Vargas",
                    Email = "carmen.jimenez@uabc.edu.mx",
                    Telefono = "(664) 567-8901",
                    Cedula = "5678901234",
                    Especialidad = "Programación Web",
                    FechaRegistro = DateTime.Now.AddMonths(-6),
                    EstadoDocenteBool = true
                },
                new E_Docente
                {
                    IdDocente = 6,
                    NombreDocente = "Luis Fernando",
                    ApellidoPaterno = "Rivera",
                    ApellidoMaterno = "Castro",
                    Email = "luis.rivera@uabc.edu.mx",
                    Telefono = "(664) 678-9012",
                    Cedula = "6789012345",
                    Especialidad = "Ciberseguridad",
                    FechaRegistro = DateTime.Now.AddMonths(-8),
                    EstadoDocenteBool = true
                }
            };
        }
    }
} 