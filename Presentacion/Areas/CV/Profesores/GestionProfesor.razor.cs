using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Presentacion.Areas.CV.Profesores
{
    partial class GestionProfesor : ComponentBase
    {
        [Parameter] public int? IdProfesor { get; set; }
        [Inject] private IJSRuntime jsRunTime { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;

        private DocenteDTO ProfesorDTO { get; set; } = new();
        private bool EsModificacion => IdProfesor.HasValue && IdProfesor.Value > 0;

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
            }
        };

        protected override async Task OnInitializedAsync()
        {
            if (EsModificacion)
            {
                await CargarProfesor();
            }
            else
            {
                ProfesorDTO = new DocenteDTO
                {
                    EstadoDocenteBool = true,
                    FechaRegistro = DateTime.Now
                };
            }
            await base.OnInitializedAsync();
        }

        private async Task CargarProfesor()
        {
            try
            {
                var profesor = ProfesoresEstaticos.FirstOrDefault(p => p.IdDocente == IdProfesor);
                if (profesor != null)
                {
                    ProfesorDTO = new DocenteDTO
                    {
                        IdDocente = profesor.IdDocente,
                        NombreDocente = profesor.NombreDocente,
                        ApellidoPaterno = profesor.ApellidoPaterno,
                        ApellidoMaterno = profesor.ApellidoMaterno,
                        Email = profesor.Email,
                        Telefono = profesor.Telefono,
                        Cedula = profesor.Cedula,
                        Especialidad = profesor.Especialidad,
                        FechaRegistro = profesor.FechaRegistro,
                        EstadoDocenteBool = profesor.EstadoDocenteBool
                    };
                }
                else
                {
                    await jsRunTime.InvokeVoidAsync("alert", "Profesor no encontrado.");
                    navigationManager.NavigateTo("ListarProfesores");
                }
            }
            catch (Exception ex)
            {
                await jsRunTime.InvokeVoidAsync("alert", $"Error al cargar el profesor: {ex.Message}");
            }
        }

        private async Task GuardarProfesor()
        {
            try
            {
                if (EsModificacion)
                {
                    // Update existing professor in static data
                    var profesorExistente = ProfesoresEstaticos.FirstOrDefault(p => p.IdDocente == ProfesorDTO.IdDocente);
                    if (profesorExistente != null)
                    {
                        profesorExistente.NombreDocente = ProfesorDTO.NombreDocente;
                        profesorExistente.ApellidoPaterno = ProfesorDTO.ApellidoPaterno;
                        profesorExistente.ApellidoMaterno = ProfesorDTO.ApellidoMaterno;
                        profesorExistente.Email = ProfesorDTO.Email;
                        profesorExistente.Telefono = ProfesorDTO.Telefono;
                        profesorExistente.Cedula = ProfesorDTO.Cedula;
                        profesorExistente.Especialidad = ProfesorDTO.Especialidad;
                        profesorExistente.EstadoDocenteBool = ProfesorDTO.EstadoDocenteBool;
                    }

                    await jsRunTime.InvokeVoidAsync("alert", "Profesor actualizado correctamente.");
                }
                else
                {
                    // Add new professor to static data
                    var nuevoId = ProfesoresEstaticos.Any() ? ProfesoresEstaticos.Max(p => p.IdDocente) + 1 : 1;
                    
                    var nuevoProfesor = new E_Docente
                    {
                        IdDocente = nuevoId,
                        NombreDocente = ProfesorDTO.NombreDocente,
                        ApellidoPaterno = ProfesorDTO.ApellidoPaterno,
                        ApellidoMaterno = ProfesorDTO.ApellidoMaterno,
                        Email = ProfesorDTO.Email,
                        Telefono = ProfesorDTO.Telefono,
                        Cedula = ProfesorDTO.Cedula,
                        Especialidad = ProfesorDTO.Especialidad,
                        FechaRegistro = DateTime.Now,
                        EstadoDocenteBool = ProfesorDTO.EstadoDocenteBool
                    };

                    ProfesoresEstaticos.Add(nuevoProfesor);
                    await jsRunTime.InvokeVoidAsync("alert", "Profesor creado correctamente.");
                }

                navigationManager.NavigateTo("ListarProfesores");
            }
            catch (Exception ex)
            {
                await jsRunTime.InvokeVoidAsync("alert", $"Error al guardar el profesor: {ex.Message}");
            }
        }
    }
} 