using Entidades.Generales;
using System.Text.RegularExpressions;
using Datos.IRepositorios.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Negocios.Repositorios.CurriculumVite
{
    public class DocenteNegocios(IDocenteRepositorio docenteRepositorio)
    {
        private readonly IDocenteRepositorio _docenteRepositorio = docenteRepositorio;

        public async Task<ResultadoAcciones> InsertarDocente(E_Docente docente)
        {
            if (docente == null)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "El docente no tiene los datos necesarios para agregarlo al sistema." },
                    Resultado = false
                };
            }

            var resultadoValidacion = await ValidarDocente(docente);
            if (!resultadoValidacion.Resultado)
                return resultadoValidacion;

            try
            {
                var resultado = await _docenteRepositorio.InsertarDocente(docente);
                return new ResultadoAcciones
                {
                    Mensajes = resultado.Mensajes,
                    Resultado = resultado.Resultado
                };
            }
            catch (Exception ex)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "Ocurrió un error inesperado al insertar el docente: ", ex.Message + "." },
                    Resultado = false
                };
            }
        }

        public async Task<ResultadoAcciones> BorrarDocente(int idDocente)
        {
            if (idDocente <= 0)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "El identificador del docente no es válido." },
                    Resultado = false
                };
            }

            try
            {
                var resultado = await _docenteRepositorio.BorrarDocente(idDocente);
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "Ocurrió un error inesperado al borrar el docente", ex.Message + "." },
                    Resultado = false
                };
            }
        }

        public async Task<ResultadoAcciones> ModificarDocente(E_Docente docente)
        {
            if (docente == null || docente.IdDocente <= 0)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "Los datos del docente son inválidos." },
                    Resultado = false
                };
            }

            var resultadoValidacion = await ValidarDocente(docente, esModificacion: true);

            if (!resultadoValidacion.Resultado)
                return resultadoValidacion;

            try
            {
                var resultado = await _docenteRepositorio.ModificarDocente(docente);
                return new ResultadoAcciones
                {
                    Mensajes = resultado.Mensajes,
                    Resultado = resultado.Resultado
                };
            }
            catch (Exception ex)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "Ocurrió un error inesperado al modificar el docente", ex.Message + "." },
                    Resultado = false
                };
            }
        }

        public async Task<ResultadoAcciones<E_Docente>> ObtenerDocente(int idDocente)
        {
            if (idDocente <= 0)
            {
                return new ResultadoAcciones<E_Docente>
                {
                    Mensajes = { "El identificador del docente no es válido." },
                    Resultado = false
                };
            }

            try
            {
                var resultado = await _docenteRepositorio.ObtenerDocentePorId(idDocente);
                return resultado;
            }
            catch (Exception)
            {
                return new ResultadoAcciones<E_Docente>
                {
                    Mensajes = { "Error al acceder a los datos del docente." },
                    Resultado = false
                };
            }
        }

        public async Task<IEnumerable<E_Docente>> ListarDocentes()
        {
            try
            {
                return await _docenteRepositorio.ObtenerTodosLosDocentes();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<IEnumerable<E_Docente>> ListarDocentes(string criterioBusqueda)
        {
            try
            {
                return await _docenteRepositorio.ObtenerDocentePorCriterio(criterioBusqueda);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<ResultadoAcciones> ValidarDocente(E_Docente docente, bool esModificacion = false)
        {
            ResultadoAcciones resultado = new();

            ValidarNumeroEmpleado(resultado, docente.NumeroEmpleado);
            ValidarNombreDocente(resultado, docente.NombreDocente);
            ValidarEmailInstitucional(resultado, docente.EmailInstitucional);
            ValidarExtension(resultado, docente.Extension);
            ValidarRFC(resultado, docente.RFC);
            ValidarCURP(resultado, docente.CURP);
            ValidarCedulaProfesional(resultado, docente.CedulaProfesional);
            ValidarForeignKeys(resultado, docente);

            if (esModificacion)
            {
                await ValidarUnicidadNumeroEmpleado(resultado, docente.NumeroEmpleado, docente.IdDocente);
                await ValidarUnicidadEmailInstitucional(resultado, docente.EmailInstitucional, docente.IdDocente);
                await ValidarUnicidadRFC(resultado, docente.RFC, docente.IdDocente);
                await ValidarUnicidadCURP(resultado, docente.CURP, docente.IdDocente);
            }
            else
            {
                await ValidarUnicidadNumeroEmpleado(resultado, docente.NumeroEmpleado);
                await ValidarUnicidadEmailInstitucional(resultado, docente.EmailInstitucional);
                await ValidarUnicidadRFC(resultado, docente.RFC);
                await ValidarUnicidadCURP(resultado, docente.CURP);
            }

            return resultado;
        }

        private static void ValidarNumeroEmpleado(ResultadoAcciones resultado, string numeroEmpleado)
        {
            if (string.IsNullOrWhiteSpace(numeroEmpleado))
            {
                resultado.Mensajes.Add("El número de empleado es obligatorio.");
                resultado.Resultado = false;
                return;
            }

            if (numeroEmpleado.Length < 3 || numeroEmpleado.Length > 20)
            {
                resultado.Mensajes.Add("El número de empleado debe tener entre 3 y 20 caracteres.");
                resultado.Resultado = false;
            }

            // Validar que solo contenga caracteres alfanuméricos
            if (!Regex.IsMatch(numeroEmpleado, @"^[a-zA-Z0-9]+$"))
            {
                resultado.Mensajes.Add("El número de empleado solo debe contener letras y números.");
                resultado.Resultado = false;
            }
        }

        private async Task ValidarUnicidadNumeroEmpleado(ResultadoAcciones resultado, string numeroEmpleado, int? idExcluido = null)
        {
            if (string.IsNullOrWhiteSpace(numeroEmpleado))
                return;

            var existe = await _docenteRepositorio.ExisteNumeroEmpleado(numeroEmpleado, idExcluido);
            if (existe)
            {
                resultado.Mensajes.Add($"Ya existe un docente con el número de empleado '{numeroEmpleado}'.");
                resultado.Resultado = false;
            }
        }

        private static void ValidarNombreDocente(ResultadoAcciones resultado, string nombreDocente)
        {
            if (string.IsNullOrWhiteSpace(nombreDocente))
            {
                resultado.Mensajes.Add("El nombre del docente es obligatorio.");
                resultado.Resultado = false;
                return;
            }

            if (nombreDocente.Length < 2 || nombreDocente.Length > 100)
            {
                resultado.Mensajes.Add("El nombre del docente debe tener entre 2 y 100 caracteres.");
                resultado.Resultado = false;
            }

            // Validar que solo contenga letras, espacios y acentos
            if (!Regex.IsMatch(nombreDocente, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                resultado.Mensajes.Add("El nombre del docente solo debe contener letras y espacios.");
                resultado.Resultado = false;
            }
        }

        private static void ValidarEmailInstitucional(ResultadoAcciones resultado, string? emailInstitucional)
        {
            if (!string.IsNullOrWhiteSpace(emailInstitucional))
            {
                // Regex más permisivo que acepta números, puntos, guiones y caracteres especiales
                var emailRegex = @"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(emailInstitucional, emailRegex))
                {
                    resultado.Mensajes.Add("El formato del email institucional no es válido.");
                    resultado.Resultado = false;
                }
            }
        }

        private async Task ValidarUnicidadEmailInstitucional(ResultadoAcciones resultado, string? emailInstitucional, int? idExcluido = null)
        {
            if (string.IsNullOrWhiteSpace(emailInstitucional))
                return;

            var existe = await _docenteRepositorio.ExisteEmailInstitucional(emailInstitucional, idExcluido);
            if (existe)
            {
                resultado.Mensajes.Add($"Ya existe un docente con el email institucional '{emailInstitucional}'.");
                resultado.Resultado = false;
            }
        }

        private static void ValidarRFC(ResultadoAcciones resultado, string? rfc)
        {
            if (!string.IsNullOrWhiteSpace(rfc))
            {
                // RFC puede ser de 10 (moral) o 13 (física) caracteres
                if (rfc.Length != 10 && rfc.Length != 13)
                {
                    resultado.Mensajes.Add("El RFC debe tener 10 o 13 caracteres.");
                    resultado.Resultado = false;
                    return;
                }

                // Validar formato básico del RFC
                var rfcRegex = @"^[A-ZÑ&]{3,4}[0-9]{6}[A-Z0-9]{3}$";
                if (!Regex.IsMatch(rfc.ToUpper(), rfcRegex))
                {
                    resultado.Mensajes.Add("El formato del RFC no es válido.");
                    resultado.Resultado = false;
                }
            }
        }

        private async Task ValidarUnicidadRFC(ResultadoAcciones resultado, string? rfc, int? idExcluido = null)
        {
            if (string.IsNullOrWhiteSpace(rfc))
                return;

            var existe = await _docenteRepositorio.ExisteRFC(rfc, idExcluido);
            if (existe)
            {
                resultado.Mensajes.Add($"Ya existe un docente con el RFC '{rfc}'.");
                resultado.Resultado = false;
            }
        }

        private static void ValidarCURP(ResultadoAcciones resultado, string? curp)
        {
            if (!string.IsNullOrWhiteSpace(curp))
            {
                if (curp.Length != 18)
                {
                    resultado.Mensajes.Add("El CURP debe tener 18 caracteres.");
                    resultado.Resultado = false;
                    return;
                }

                // Validar formato básico del CURP
                var curpRegex = @"^[A-Z]{4}[0-9]{6}[HM][A-Z]{5}[0-9A-Z][0-9]$";
                if (!Regex.IsMatch(curp.ToUpper(), curpRegex))
                {
                    resultado.Mensajes.Add("El formato del CURP no es válido.");
                    resultado.Resultado = false;
                }
            }
        }

        private async Task ValidarUnicidadCURP(ResultadoAcciones resultado, string? curp, int? idExcluido = null)
        {
            if (string.IsNullOrWhiteSpace(curp))
                return;

            var existe = await _docenteRepositorio.ExisteCURP(curp, idExcluido);
            if (existe)
            {
                resultado.Mensajes.Add($"Ya existe un docente con el CURP '{curp}'.");
                resultado.Resultado = false;
            }
        }

        private static void ValidarCedulaProfesional(ResultadoAcciones resultado, string? cedulaProfesional)
        {
            if (!string.IsNullOrWhiteSpace(cedulaProfesional))
            {
                if (cedulaProfesional.Length < 6 || cedulaProfesional.Length > 12)
                {
                    resultado.Mensajes.Add("La cédula profesional debe tener entre 6 y 12 caracteres.");
                    resultado.Resultado = false;
                }

                // Validar que solo contenga números
                if (!Regex.IsMatch(cedulaProfesional, @"^[0-9]+$"))
                {
                    resultado.Mensajes.Add("La cédula profesional solo debe contener números.");
                    resultado.Resultado = false;
                }
            }
        }

        private static void ValidarExtension(ResultadoAcciones resultado, string? extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                if (extension.Length > 14)
                {
                    resultado.Mensajes.Add("La extensión debe tener como máximo 14 caracteres.");
                    resultado.Resultado = false;
                }
            }
        }

        private static void ValidarForeignKeys(ResultadoAcciones resultado, E_Docente docente)
        {
            if (docente.IdSexo <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar un sexo válido.");
                resultado.Resultado = false;
            }

            if (docente.IdEstadoCivil <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar un estado civil válido.");
                resultado.Resultado = false;
            }

            if (docente.IdCategoria <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar una categoría válida.");
                resultado.Resultado = false;
            }

            if (docente.IdNombramiento <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar un nombramiento válido.");
                resultado.Resultado = false;
            }

            if (docente.IdEscolaridad <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar una escolaridad válida.");
                resultado.Resultado = false;
            }

            if (docente.IdNivelSNI <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar un nivel SNI válido.");
                resultado.Resultado = false;
            }

            if (docente.IdPRODEP <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar un PRODEP válido.");
                resultado.Resultado = false;
            }

            if (docente.IdCarrera <= 0)
            {
                resultado.Mensajes.Add("Debe seleccionar una carrera válida.");
                resultado.Resultado = false;
            }
        }

        public async Task<ResultadoAcciones> ActualizarUrlFoto(int idDocente, string urlFoto)
        {
            if (idDocente <= 0)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "El identificador del docente no es válido." },
                    Resultado = false
                };
            }

            if (string.IsNullOrWhiteSpace(urlFoto))
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "La URL de la foto no puede estar vacía." },
                    Resultado = false
                };
            }

            try
            {
                return await _docenteRepositorio.ActualizarUrlFoto(idDocente, urlFoto);
            }
            catch (Exception ex)
            {
                return new ResultadoAcciones
                {
                    Mensajes = { "Error al actualizar la URL de la foto: " + ex.Message },
                    Resultado = false
                };
            }
        }
    }
} 