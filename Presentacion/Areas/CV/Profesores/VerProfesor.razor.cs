using Entidades.Modelos.CurriculumVite;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Presentacion.Areas.CV.Profesores
{
    partial class VerProfesor : ComponentBase
    {
        [Parameter] public int IdProfesor { get; set; }
        [Inject] private IJSRuntime jsRunTime { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;

        private E_Docente? Profesor { get; set; }

        // Static data since database is not configured
        private static List<E_Docente> ProfesoresEstaticos = new List<E_Docente>
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

        protected override async Task OnInitializedAsync()
        {
            await CargarProfesor();
            await base.OnInitializedAsync();
        }

        private async Task CargarProfesor()
        {
            try
            {
                Profesor = ProfesoresEstaticos.FirstOrDefault(p => p.IdDocente == IdProfesor);
                
                if (Profesor == null)
                {
                    await jsRunTime.InvokeVoidAsync("console.log", $"Profesor con ID {IdProfesor} no encontrado");
                }
            }
            catch (Exception ex)
            {
                await jsRunTime.InvokeVoidAsync("console.error", $"Error al cargar el profesor: {ex.Message}");
            }
        }
    }
} 