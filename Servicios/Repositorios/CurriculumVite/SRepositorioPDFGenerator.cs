using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Elements;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using System.Net.Http;

namespace Servicios.Repositorios.CurriculumVite
{
    public class SRepositorioPDFGenerator : ISRepositorioPDFGenerator
    {
        public SRepositorioPDFGenerator()
        {
            // Configurar QuestPDF para uso sin licencia comercial
            QuestPDF.Settings.License = LicenseType.Community;
            // Habilitar debugging para identificar problemas de layout
            QuestPDF.Settings.EnableDebugging = true;
        }

        public byte[] GenerarCVPDF(
            DocenteDTO docente,
            List<EducacionDTO>? educaciones = null,
            List<ExperienciaDTO>? experiencias = null,
            List<PublicacionDTO>? publicaciones = null,
            List<ProyectoDTO>? proyectos = null,
            List<TesisDirigidaDTO>? tesisDirigidas = null,
            List<DistincionDTO>? distinciones = null)
        {
            // Cargar logos automáticamente con fallback
            Console.WriteLine("⠀ GENERANDO CV PDF - CARGA DE LOGOS...");
            var logos = CargarLogosDesdeArchivos();
            
            if (logos.Count == 0)
            {
                Console.WriteLine("⠀ LOGOS LOCALES FALLARON - USANDO GOOGLE DRIVE");
                logos = CargarLogosDesdeGoogleDrive();
            }
            
            Console.WriteLine($"📊 LOGOS PARA PDF: {logos.Count}");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10).FontColor(Colors.Black));

                    page.Header().Element(container => CreateHeaderConLogos(container, docente, null, logos));
                    page.Content().Element(content => CreateContent(content, docente, educaciones, experiencias, publicaciones, proyectos, tesisDirigidas, distinciones));
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerarCVPDFConContactos(
            DocenteDTO docente,
            List<EducacionDTO>? educaciones = null,
            List<ExperienciaDTO>? experiencias = null,
            List<PublicacionDTO>? publicaciones = null,
            List<ProyectoDTO>? proyectos = null,
            List<TesisDirigidaDTO>? tesisDirigidas = null,
            List<DistincionDTO>? distinciones = null,
            List<ContactoDocenteDTO>? contactos = null,
            Dictionary<string, byte[]>? logos = null)
        {
            // Si no se proporcionan logos o están vacíos, cargarlos automáticamente
            Console.WriteLine("🔥🔥🔥 VERIFICANDO LOGOS - ENTRADA AL MÉTODO 🔥🔥🔥");
            Console.WriteLine($"🔍 Logos recibidos como parámetro: {(logos == null ? "NULL" : logos.Count.ToString())}");
            
            if (logos == null || logos.Count == 0)
            {
                Console.WriteLine("⠀ INICIANDO CARGA DE LOGOS AUTOMÁTICA (NULL O VACÍO)...");
                logos = CargarLogosDesdeArchivos();
                Console.WriteLine($"⠀ DESPUÉS CargarLogosDesdeArchivos: {logos.Count} logos");
                
                // Si no se cargaron logos locales, usar fallback de Google Drive
                if (logos.Count == 0)
                {
                    Console.WriteLine("⠀ ACTIVANDO FALLBACK - CARGANDO DESDE GOOGLE DRIVE");
                    logos = CargarLogosDesdeGoogleDrive();
                    Console.WriteLine($"⠀ DESPUÉS CargarLogosDesdeGoogleDrive: {logos.Count} logos");
                }
                
                Console.WriteLine($"📊 LOGOS FINALES DISPONIBLES: {logos.Count}");
            }
            else
            {
                Console.WriteLine($"⠀ LOGOS YA PROPORCIONADOS Y NO VACÍOS: {logos.Count}");
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10).FontColor(Colors.Black));

                    page.Header().Element(container => CreateHeaderConLogos(container, docente, contactos, logos));
                    page.Content().Element(content => CreateContentMejorado(content, docente, educaciones, experiencias, publicaciones, proyectos, tesisDirigidas, distinciones));
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerarCV(
            DocenteDTO docente,
            List<EducacionDTO> educaciones,
            List<ExperienciaDTO> experiencias,
            List<PublicacionDTO> publicaciones,
            List<ProyectoDTO> proyectos,
            List<TesisDirigidaDTO> tesisDirigidas,
            List<DistincionDTO> distinciones)
        {
            return GenerarCVPDF(docente, educaciones, experiencias, publicaciones, proyectos, tesisDirigidas, distinciones);
        }

        public byte[] GenerarCVConContactos(
            DocenteDTO docente,
            List<EducacionDTO> educaciones,
            List<ExperienciaDTO> experiencias,
            List<PublicacionDTO> publicaciones,
            List<ProyectoDTO> proyectos,
            List<TesisDirigidaDTO> tesisDirigidas,
            List<DistincionDTO> distinciones,
            List<ContactoDocenteDTO> contactos,
            Dictionary<string, byte[]>? logos = null)
        {
            return GenerarCVPDFConContactos(docente, educaciones, experiencias, publicaciones, proyectos, tesisDirigidas, distinciones, contactos, logos);
        }

        public byte[] GenerarCVDemo()
        {
            var docenteDemo = CrearDocenteDemo();
            var educacionesDemo = CrearEducacionesDemo();
            var experienciasDemo = CrearExperienciasDemo();
            var publicacionesDemo = CrearPublicacionesDemo();
            var proyectosDemo = CrearProyectosDemo();
            var tesisDemo = CrearTesisDemo();
            var distincionesDemo = CrearDistincionesDemo();
            var contactosDemo = new List<ContactoDocenteDTO>();

            // Cargar logos automáticamente con fallback
            Console.WriteLine("⠀ GENERANDO CV DEMO - CARGA DE LOGOS...");
            var logos = CargarLogosDesdeArchivos();
            
            if (logos.Count == 0)
            {
                Console.WriteLine("⠀ DEMO - LOGOS LOCALES FALLARON - USANDO GOOGLE DRIVE");
                logos = CargarLogosDesdeGoogleDrive();
            }

            return GenerarCVPDFConContactos(docenteDemo, educacionesDemo, experienciasDemo, publicacionesDemo, proyectosDemo, tesisDemo, distincionesDemo, contactosDemo, logos);
        }

        private void CreateHeaderSimple(IContainer container, DocenteDTO? docente = null)
        {
            container.Column(column =>
            {
                // Encabezado básico
                column.Item().Row(row =>
                {
                    // Logo UABC (texto)
                    row.RelativeItem(2).Column(leftCol =>
                    {
                        leftCol.Item().Text("UNIVERSIDAD AUTÓNOMA DE BAJA CALIFORNIA")
                            .FontSize(12).Bold().FontColor("#2D6B3C");
                        leftCol.Item().Text("Facultad de Ingeniería y Diseño")
                            .FontSize(10).FontColor("#2D6B3C");
                    });

                    // Información de contacto
                    row.RelativeItem(1).Column(rightCol =>
                    {
                        rightCol.Item().AlignRight().Text("Datos del contacto:")
                            .FontSize(9).Bold().FontColor("#2D6B3C");
                        
                        var email = !string.IsNullOrEmpty(docente?.EmailInstitucional) 
                            ? docente.EmailInstitucional 
                            : "correo@uabc.edu.mx";
                        rightCol.Item().AlignRight().Text($"✉ {email}")
                            .FontSize(8).FontColor("#004c26");
                    });
                });

                // Línea separadora
                column.Item().PaddingVertical(5).LineHorizontal(1).LineColor("#4BB463");
            });
        }

        private void CreateContent(IContainer container, DocenteDTO docente,
            List<EducacionDTO>? educaciones, List<ExperienciaDTO>? experiencias,
            List<PublicacionDTO>? publicaciones, List<ProyectoDTO>? proyectos,
            List<TesisDirigidaDTO>? tesisDirigidas, List<DistincionDTO>? distinciones)
        {
            // Generar numeración dinámica basada en las secciones con contenido
            var sectionCounter = 0;
            var romanNumerals = new[] { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            
            string GetNextSectionNumber() => romanNumerals[++sectionCounter];

            container.Column(column =>
            {
                // Información principal del docente
                column.Item().Row(row =>
                {
                    // Foto o iniciales
                    row.ConstantItem(100).Height(120).Column(fotoColumn =>
                    {
                        if (!string.IsNullOrEmpty(docente.URLFoto))
                        {
                            try
                            {
                                var fotoBytes = DescargarImagenDesdeDrive(docente.URLFoto);
                                if (fotoBytes != null && fotoBytes.Length > 0)
                                {
                                    fotoColumn.Item().Height(120).Width(100)
                                        .Border(2).BorderColor("#2D6B3C")
                                        .Background("#FFFFFF")
                                        .Padding(3)
                                        .Image(fotoBytes)
                                        .FitArea();
                                }
                                else
                                {
                                    fotoColumn.Item().Height(120).Background("#F4F4F4").Border(2).BorderColor("#2D6B3C")
                                        .AlignCenter().AlignMiddle().Text(GetIniciales(docente.NombreDocente, docente.PaternoDocente))
                                        .FontSize(24).Bold().FontColor("#2D6B3C");
                                }
                            }
                            catch
                            {
                                fotoColumn.Item().Height(120).Background("#F4F4F4").Border(2).BorderColor("#2D6B3C")
                                    .AlignCenter().AlignMiddle().Text(GetIniciales(docente.NombreDocente, docente.PaternoDocente))
                                    .FontSize(24).Bold().FontColor("#2D6B3C");
                            }
                        }
                        else
                        {
                            fotoColumn.Item().Height(120).Background("#F4F4F4").Border(2).BorderColor("#2D6B3C")
                                .AlignCenter().AlignMiddle().Text(GetIniciales(docente.NombreDocente, docente.PaternoDocente))
                                .FontSize(24).Bold().FontColor("#2D6B3C");
                        }
                    });

                    // Información básica
                    row.RelativeItem().PaddingLeft(20).Column(info =>
                    {
                        info.Item().Text($"{docente.NombreCompleto}")
                            .FontSize(20).Bold().FontColor("#2D6B3C");
                        info.Item().Text($"{docente.NombreCarrera}")
                            .FontSize(14).FontColor("#666666");
                        info.Item().PaddingTop(5).Text($"No. Empleado: {docente.NumeroEmpleado}")
                            .FontSize(11);
                        info.Item().Text($"Carrera: {docente.NombreCarrera}")
                            .FontSize(11);
                        if (!string.IsNullOrEmpty(docente.EmailInstitucional))
                        {
                            info.Item().Text($"Email: {docente.EmailInstitucional}")
                                .FontSize(11).FontColor("#004c26");
                        }
                        if (!string.IsNullOrEmpty(docente.Extension))
                        {
                            info.Item().Text($"Extensión: {docente.Extension}")
                                .FontSize(11);
                        }
                    });
                });

                // Semblanza (si existe)
                if (!string.IsNullOrEmpty(docente.SembalzaDocente))
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text("SEMBLANZA")
                        .FontSize(12).Bold().FontColor(Colors.White);
                    column.Item().PaddingTop(5).PaddingLeft(10).PaddingRight(10)
                        .Border(1).BorderColor("#E0E0E0").Background("#FAFAFA")
                        .Padding(10).Text(docente.SembalzaDocente)
                        .FontSize(11).LineHeight(1.4f);
                }

                // Formación Académica
                if (educaciones?.Any() == true)
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text($"{GetNextSectionNumber()}. FORMACIÓN ACADÉMICA")
                        .FontSize(14).Bold().FontColor(Colors.White);
                    
                    foreach (var educacion in educaciones.OrderByDescending(e => e.AnioFin))
                    {
                        column.Item().PaddingTop(10).Row(row =>
                        {
                            row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"{educacion.Nivel}: {educacion.Titulo}")
                                    .Bold().FontSize(11);
                                if (!string.IsNullOrEmpty(educacion.Institucion))
                                {
                                    col.Item().Text($"{educacion.Institucion}")
                                        .FontSize(10).FontColor("#666666");
                                }
                                if (educacion.AnioInicio.HasValue || educacion.AnioFin.HasValue)
                                {
                                    var periodo = $"{educacion.AnioInicio}-{educacion.AnioFin}";
                                    col.Item().Text(periodo).FontSize(10).Italic();
                                }
                            });
                        });
                    }
                }

                // Experiencia Profesional
                if (experiencias?.Any() == true)
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text($"{GetNextSectionNumber()}. EXPERIENCIA PROFESIONAL")
                        .FontSize(14).Bold().FontColor(Colors.White);
                    
                    foreach (var experiencia in experiencias.OrderByDescending(e => e.FechaInicio))
                    {
                        column.Item().PaddingTop(10).Row(row =>
                        {
                            row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"{experiencia.Puesto}")
                                    .Bold().FontSize(11);
                                if (!string.IsNullOrEmpty(experiencia.Institucion))
                                {
                                    col.Item().Text($"{experiencia.Institucion}")
                                        .FontSize(10).FontColor("#666666");
                                }
                                if (experiencia.FechaInicio.HasValue)
                                {
                                    var periodo = experiencia.FechaFin.HasValue 
                                        ? $"{experiencia.FechaInicio:MM/yyyy} - {experiencia.FechaFin:MM/yyyy}"
                                        : $"{experiencia.FechaInicio:MM/yyyy} - Presente";
                                    col.Item().Text(periodo).FontSize(10).Italic();
                                }
                                if (!string.IsNullOrEmpty(experiencia.Descripcion))
                                {
                                    col.Item().PaddingTop(2).Text(experiencia.Descripcion)
                                        .FontSize(10).LineHeight(1.3f);
                                }
                            });
                        });
                    }
                }

                // Proyectos de Investigación
                if (proyectos?.Any() == true)
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text($"{GetNextSectionNumber()}. PROYECTOS DE INVESTIGACIÓN")
                        .FontSize(14).Bold().FontColor(Colors.White);
                    
                    foreach (var proyecto in proyectos.OrderByDescending(p => p.PeriodoFin))
                    {
                        column.Item().PaddingTop(10).Row(row =>
                        {
                            row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"{proyecto.Titulo}")
                                    .Bold().FontSize(11);
                                if (!string.IsNullOrEmpty(proyecto.Rol))
                                {
                                    col.Item().Text($"Rol: {proyecto.Rol}")
                                        .FontSize(10).FontColor("#666666");
                                }
                                if (!string.IsNullOrEmpty(proyecto.Institucion))
                                {
                                    col.Item().Text($"Institución: {proyecto.Institucion}")
                                        .FontSize(10);
                                }
                                if (!string.IsNullOrEmpty(proyecto.Financiamiento))
                                {
                                    col.Item().Text($"Financiamiento: {proyecto.Financiamiento}")
                                        .FontSize(10);
                                }
                                if (proyecto.PeriodoInicio.HasValue || proyecto.PeriodoFin > 0)
                                {
                                    col.Item().Text($"Período: {proyecto.PeriodoInicio}-{proyecto.PeriodoFin}")
                                        .FontSize(10).Italic();
                                }
                            });
                        });
                    }
                }

                // Publicaciones
                if (publicaciones?.Any() == true)
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text($"{GetNextSectionNumber()}. PUBLICACIONES")
                        .FontSize(14).Bold().FontColor(Colors.White);
                    
                    foreach (var publicacion in publicaciones.OrderByDescending(p => p.Anio))
                    {
                        column.Item().PaddingTop(8).Row(row =>
                        {
                            row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"{publicacion.Titulo}")
                                    .Bold().FontSize(11);
                                if (!string.IsNullOrEmpty(publicacion.Autores))
                                {
                                    col.Item().Text($"Autores: {publicacion.Autores}")
                                        .FontSize(10);
                                }
                                if (!string.IsNullOrEmpty(publicacion.Fuente))
                                {
                                    col.Item().Text($"{publicacion.Fuente}")
                                        .FontSize(10).Italic();
                                }
                                if (publicacion.Anio.HasValue)
                                {
                                    col.Item().Text($"Año: {publicacion.Anio}")
                                        .FontSize(10).FontColor("#666666");
                                }
                            });
                        });
                    }
                }

                // Dirección de Tesis
                if (tesisDirigidas?.Any() == true)
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text($"{GetNextSectionNumber()}. DIRECCIÓN DE TESIS")
                        .FontSize(14).Bold().FontColor(Colors.White);
                    
                    foreach (var tesis in tesisDirigidas.OrderByDescending(t => t.Anio))
                    {
                        column.Item().PaddingTop(8).Row(row =>
                        {
                            row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"{tesis.Titulo}")
                                    .Bold().FontSize(11);
                                if (!string.IsNullOrEmpty(tesis.Autor))
                                {
                                    col.Item().Text($"Autor: {tesis.Autor}")
                                        .FontSize(10);
                                }
                                if (!string.IsNullOrEmpty(tesis.Nivel))
                                {
                                    col.Item().Text($"Nivel: {tesis.Nivel}")
                                        .FontSize(10);
                                }
                                if (!string.IsNullOrEmpty(tesis.Universidad))
                                {
                                    col.Item().Text($"Universidad: {tesis.Universidad}")
                                        .FontSize(10);
                                }
                                if (tesis.Anio.HasValue)
                                {
                                    col.Item().Text($"Año: {tesis.Anio}")
                                        .FontSize(10).FontColor("#666666");
                                }
                            });
                        });
                    }
                }

                // Distinciones y Reconocimientos
                if (distinciones?.Any() == true)
                {
                    column.Item().PaddingTop(20).Background("#2D6B3C").Padding(8).Text($"{GetNextSectionNumber()}. DISTINCIONES Y RECONOCIMIENTOS")
                        .FontSize(14).Bold().FontColor(Colors.White);
                    
                    foreach (var distincion in distinciones.OrderByDescending(d => d.Anio))
                    {
                        column.Item().PaddingTop(8).Row(row =>
                        {
                            row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"{distincion.Nombre}")
                                    .Bold().FontSize(11);
                                if (!string.IsNullOrEmpty(distincion.Institucion))
                                {
                                    col.Item().Text($"Institución: {distincion.Institucion}")
                                        .FontSize(10);
                                }
                                if (!string.IsNullOrEmpty(distincion.Descripcion))
                                {
                                    col.Item().Text($"{distincion.Descripcion}")
                                        .FontSize(10);
                                }
                                if (distincion.Anio.HasValue)
                                {
                                    col.Item().Text($"Año: {distincion.Anio}")
                                        .FontSize(10).FontColor("#666666");
                                }
                            });
                        });
                    }
                }
            });
        }



        private void CreateHeaderConLogos(IContainer container, DocenteDTO docente, List<ContactoDocenteDTO>? contactos, Dictionary<string, byte[]>? logos = null)
        {
            container.Column(mainColumn =>
            {
                // Header superior con logos y título institucional
                mainColumn.Item().Height(100).Row(headerRow =>
                {

                    // Títulos institucionales centrados
                    headerRow.RelativeItem().AlignCenter().AlignMiddle().Column(tituloColumn =>
                    {
                        tituloColumn.Item().AlignCenter().Text("Universidad Autónoma de Baja California")
                            .FontFamily("Times New Roman").FontSize(16).Bold().FontColor("#2D6B3C");
                        tituloColumn.Item().AlignCenter().Text("Facultad de Ingeniería y Diseño")
                            .FontFamily("Times New Roman").FontSize(14).FontColor("#F4BF3A");
                    });

                    // Logos a la derecha
                    headerRow.ConstantItem(180).Row(logosRow =>
                    {
                        // Logo FIAD
                        logosRow.RelativeItem().AlignCenter().AlignMiddle().Column(fiadColumn =>
                        {
                            if (logos != null && logos.ContainsKey("fiad") && logos["fiad"]?.Length > 0)
                            {
                                Console.WriteLine($"⠀ Renderizando logo FIAD: {logos["fiad"].Length} bytes");
                                fiadColumn.Item().Height(70).Width(70)
                                    .Background("#FFFFFF")
                                    .Padding(2)
                                    .Image(logos["fiad"])
                                    .FitArea();
                            }
                            else
                            {
                                fiadColumn.Item().Height(70).Width(70)
                                    .Border(1).BorderColor("#2D6B3C").Background("#F8F8F8")
                                    .AlignCenter().AlignMiddle().Text("FIAD")
                                    .FontSize(10).Bold().FontColor("#2D6B3C");
                            }
                        });

                        // Logo UABC
                        logosRow.RelativeItem().AlignCenter().AlignMiddle().Column(uabcColumn =>
                        {
                            if (logos != null && logos.ContainsKey("uabc") && logos["uabc"]?.Length > 0)
                            {
                                Console.WriteLine($"⠀ Renderizando logo UABC: {logos["uabc"].Length} bytes");
                                uabcColumn.Item().Height(70).Width(70)
                                    .Background("#FFFFFF")
                                    .Padding(2)
                                    .Image(logos["uabc"])
                                    .FitArea();
                            }
                            else
                            {
                                uabcColumn.Item().Height(70).Width(70)
                                    .Border(1).BorderColor("#2D6B3C").Background("#F8F8F8")
                                    .AlignCenter().AlignMiddle().Text("UABC")
                                    .FontSize(10).Bold().FontColor("#2D6B3C");
                            }
                        });
                    });
                });

                // Línea separadora
                mainColumn.Item().PaddingTop(10).LineHorizontal(1).LineColor("#2D6B3C");

                // Información del docente y contactos
                mainColumn.Item().PaddingTop(10).Background("#2D6B3C").Padding(15).Row(infoRow =>
                {
                    // Información principal del docente
                    infoRow.RelativeItem(3).Column(docenteInfo =>
                    {
                        docenteInfo.Item().Text($"Mtro. {docente.NombreCompleto}")
                            .FontSize(18).Bold().FontColor(Colors.White);
                        docenteInfo.Item().PaddingTop(3).Text(docente.NombreCarrera ?? "Sin carrera asignada")
                            .FontSize(12).FontColor(Colors.White);
                        docenteInfo.Item().PaddingTop(2).Text($"No. Empleado: {docente.NumeroEmpleado}")
                            .FontSize(10).FontColor(Colors.White);
                    });

                    // Contactos del docente
                    infoRow.RelativeItem(2).Column(contactoColumn =>
                    {
                        contactoColumn.Item().Text("Contactos del Docente")
                            .FontSize(12).Bold().FontColor(Colors.White);
                        
                        // Email institucional
                        if (!string.IsNullOrEmpty(docente.EmailInstitucional))
                        {
                            contactoColumn.Item().PaddingTop(3).Text($"✉ {docente.EmailInstitucional}")
                                .FontSize(10).FontColor(Colors.White);
                        }
                        
                        // Contactos adicionales del docente con enlaces
                        if (contactos?.Any() == true)
                        {
                            foreach (var contacto in contactos.Take(3)) // Máximo 3 contactos
                            {
                                var tipoIcono = GetIconoContacto(contacto.NombreTipoContacto);
                                var urlMostrar = contacto.Url;
                                
                                // Acortar URL si es muy larga
                                if (urlMostrar.Length > 35)
                                {
                                    urlMostrar = urlMostrar.Substring(0, 32) + "...";
                                }
                                
                                contactoColumn.Item().PaddingTop(2).Text($"{tipoIcono}: {urlMostrar}")
                                    .FontSize(9).FontColor(Colors.White);
                            }
                        }
                    });
                });

                // Línea separadora final
                mainColumn.Item().PaddingTop(5).LineHorizontal(2).LineColor("#F4BF3A");
            });
        }

        private void CreateContentMejorado(IContainer container, DocenteDTO docente,
            List<EducacionDTO>? educaciones, List<ExperienciaDTO>? experiencias,
            List<PublicacionDTO>? publicaciones, List<ProyectoDTO>? proyectos,
            List<TesisDirigidaDTO>? tesisDirigidas, List<DistincionDTO>? distinciones)
        {
            // Generar numeración dinámica basada en las secciones con contenido
            var sectionCounter = 0;
            var romanNumerals = new[] { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            
            string GetNextSectionNumber() => romanNumerals[++sectionCounter];

            container.Column(column =>
            {
                // Semblanza en formato de dos columnas
                if (!string.IsNullOrEmpty(docente.SembalzaDocente))
                {
                    column.Item().PaddingTop(10).Row(semblanzaRow =>
                    {
                        // Columna izquierda con imagen/iniciales
                        semblanzaRow.ConstantItem(150).Column(leftCol =>
                        {
                            if (!string.IsNullOrEmpty(docente.URLFoto))
                            {
                                try
                                {
                                    var fotoBytes = DescargarImagenDesdeDrive(docente.URLFoto);
                                    if (fotoBytes != null && fotoBytes.Length > 0)
                                    {
                                        leftCol.Item().Height(120).Width(120)
                                            .Border(2).BorderColor("#2D6B3C")
                                            .Background("#FFFFFF")
                                            .Padding(3)
                                            .Image(fotoBytes)
                                            .FitArea();
                                    }
                                    else
                                    {
                                        leftCol.Item().Height(120).Background("#F0F0F0").Border(2).BorderColor("#2D6B3C")
                                            .AlignCenter().AlignMiddle().Text(GetIniciales(docente.NombreDocente, docente.PaternoDocente))
                                            .FontSize(24).Bold().FontColor("#2D6B3C");
                                    }
                                }
                                catch
                                {
                                    leftCol.Item().Height(120).Background("#F0F0F0").Border(2).BorderColor("#2D6B3C")
                                        .AlignCenter().AlignMiddle().Text(GetIniciales(docente.NombreDocente, docente.PaternoDocente))
                                        .FontSize(24).Bold().FontColor("#2D6B3C");
                                }
                            }
                            else
                            {
                                leftCol.Item().Height(120).Background("#F0F0F0").Border(2).BorderColor("#2D6B3C")
                                    .AlignCenter().AlignMiddle().Text(GetIniciales(docente.NombreDocente, docente.PaternoDocente))
                                    .FontSize(24).Bold().FontColor("#2D6B3C");
                            }
                                
                            leftCol.Item().PaddingTop(10).AlignCenter().Text("Semblanza")
                                .FontSize(48).Bold().FontColor("#E0E0E0").Italic();
                        });

                        // Columna derecha con semblanza
                        semblanzaRow.RelativeItem().PaddingLeft(20).Column(rightCol =>
                        {
                            rightCol.Item().Text("Semblanza")
                                .FontSize(16).Bold().FontColor("#2D6B3C");
                            
                            rightCol.Item().PaddingTop(10).Text(docente.SembalzaDocente)
                                .FontSize(11).LineHeight(1.4f).Justify();
                        });
                    });
                }

                // Formación Académica
                if (educaciones?.Any() == true)
                {
                    column.Item().PaddingTop(25).Column(educacionCol =>
                    {
                        educacionCol.Item().Text($"{GetNextSectionNumber()}. Formación académica")
                            .FontSize(16).Bold().FontColor("#2D6B3C");
                        
                        foreach (var educacion in educaciones.OrderByDescending(e => e.AnioFin))
                        {
                            educacionCol.Item().PaddingTop(8).Row(row =>
                            {
                                row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"{educacion.Nivel}: {educacion.Titulo}")
                                        .Bold().FontSize(11);
                                    if (!string.IsNullOrEmpty(educacion.Institucion))
                                    {
                                        col.Item().Text($"{educacion.Institucion}")
                                            .FontSize(10).FontColor("#666666");
                                    }
                                    if (educacion.AnioInicio.HasValue || educacion.AnioFin.HasValue)
                                    {
                                        var periodo = $"{educacion.AnioInicio}-{educacion.AnioFin}";
                                        col.Item().Text(periodo).FontSize(10).Italic();
                                    }
                                });
                            });
                        }
                    });
                }

                // Experiencia Profesional
                if (experiencias?.Any() == true)
                {
                    column.Item().PaddingTop(20).Column(expCol =>
                    {
                        expCol.Item().Text($"{GetNextSectionNumber()}. Experiencia Profesional")
                            .FontSize(14).Bold().FontColor("#2D6B3C");
                        
                        foreach (var experiencia in experiencias.OrderByDescending(e => e.FechaInicio))
                        {
                            expCol.Item().PaddingTop(10).Row(row =>
                            {
                                row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"{experiencia.Puesto}")
                                        .Bold().FontSize(11);
                                    if (!string.IsNullOrEmpty(experiencia.Institucion))
                                    {
                                        col.Item().Text($"{experiencia.Institucion}")
                                            .FontSize(10).FontColor("#666666");
                                    }
                                    if (experiencia.FechaInicio.HasValue)
                                    {
                                        var periodo = experiencia.FechaFin.HasValue 
                                            ? $"{experiencia.FechaInicio:MM/yyyy} - {experiencia.FechaFin:MM/yyyy}"
                                            : $"{experiencia.FechaInicio:MM/yyyy} - Presente";
                                        col.Item().Text(periodo).FontSize(10).Italic();
                                    }
                                    if (!string.IsNullOrEmpty(experiencia.Descripcion))
                                    {
                                        col.Item().PaddingTop(2).Text(experiencia.Descripcion)
                                            .FontSize(10).LineHeight(1.3f);
                                    }
                                });
                            });
                        }
                    });
                }

                // Proyectos
                if (proyectos?.Any() == true)
                {
                    column.Item().PaddingTop(20).Column(proyCol =>
                    {
                        proyCol.Item().Text($"{GetNextSectionNumber()}. Proyectos")
                            .FontSize(14).Bold().FontColor("#2D6B3C");
                        
                        foreach (var proyecto in proyectos.OrderByDescending(p => p.PeriodoInicio))
                        {
                            proyCol.Item().PaddingTop(10).Row(row =>
                            {
                                row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"{proyecto.Titulo}")
                                        .Bold().FontSize(11);
                                    if (!string.IsNullOrEmpty(proyecto.Rol))
                                    {
                                        col.Item().Text($"Rol: {proyecto.Rol}")
                                            .FontSize(10).FontColor("#666666");
                                    }
                                    if (!string.IsNullOrEmpty(proyecto.Institucion))
                                    {
                                        col.Item().Text($"Institución: {proyecto.Institucion}")
                                            .FontSize(10);
                                    }
                                    if (proyecto.PeriodoInicio.HasValue)
                                    {
                                        var periodo = $"{proyecto.PeriodoInicio} - {proyecto.PeriodoFin}";
                                        col.Item().Text(periodo).FontSize(10).Italic();
                                    }
                                });
                            });
                        }
                    });
                }

                // Publicaciones organizadas por tipo
                if (publicaciones?.Any() == true)
                {
                    column.Item().PaddingTop(20).Column(pubCol =>
                    {
                        pubCol.Item().Text($"{GetNextSectionNumber()}. Publicaciones")
                            .FontSize(14).Bold().FontColor("#2D6B3C");
                        
                        // Agrupar publicaciones por tipo
                        var publicacionesAgrupadas = publicaciones
                            .GroupBy(p => p.TipoPublicacion ?? "Otros")
                            .OrderBy(g => g.Key);
                        
                        foreach (var grupo in publicacionesAgrupadas)
                        {
                            // Subtítulo del tipo de publicación
                            pubCol.Item().PaddingTop(15).Text($"- {grupo.Key}")
                                .FontSize(12).Bold().FontColor("#4BB463");
                            
                            // Publicaciones de este tipo
                            foreach (var publicacion in grupo.OrderByDescending(p => p.Anio))
                            {
                                pubCol.Item().PaddingTop(8).Row(row =>
                                {
                                    row.ConstantItem(25).Text("•").FontSize(10).FontColor("#666666");
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().Text($"{publicacion.Titulo}")
                                            .Bold().FontSize(11);
                                        
                                        // Formato APA
                                        var formatoAPA = GenerarFormatoAPA(publicacion);
                                        if (!string.IsNullOrEmpty(formatoAPA))
                                        {
                                            col.Item().PaddingTop(3).Text($"Formato APA: {formatoAPA}")
                                                .FontSize(9).FontColor("#004c26").Italic();
                                        }
                                        
                                        if (!string.IsNullOrEmpty(publicacion.Autores))
                                        {
                                            col.Item().Text($"Autores: {publicacion.Autores}")
                                                .FontSize(10).FontColor("#666666");
                                        }
                                        if (!string.IsNullOrEmpty(publicacion.Fuente))
                                        {
                                            col.Item().Text($"{publicacion.Fuente}")
                                                .FontSize(10);
                                        }
                                        if (publicacion.Anio.HasValue)
                                        {
                                            col.Item().Text($"Año: {publicacion.Anio}")
                                                .FontSize(10).Italic();
                                        }
                                    });
                                });
                            }
                        }
                    });
                }

                // Dirección de Tesis
                if (tesisDirigidas?.Any() == true)
                {
                    column.Item().PaddingTop(20).Column(tesisCol =>
                    {
                        tesisCol.Item().Text($"{GetNextSectionNumber()}. Dirección de Tesis")
                            .FontSize(14).Bold().FontColor("#2D6B3C");
                        
                        foreach (var tesis in tesisDirigidas.OrderByDescending(t => t.Anio))
                        {
                            tesisCol.Item().PaddingTop(10).Row(row =>
                            {
                                row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"{tesis.Titulo}")
                                        .Bold().FontSize(11);
                                    if (!string.IsNullOrEmpty(tesis.Autor))
                                    {
                                        col.Item().Text($"Autor: {tesis.Autor}")
                                            .FontSize(10).FontColor("#666666");
                                    }
                                    if (!string.IsNullOrEmpty(tesis.Nivel))
                                    {
                                        col.Item().Text($"Nivel: {tesis.Nivel}")
                                            .FontSize(10);
                                    }
                                    if (!string.IsNullOrEmpty(tesis.Universidad))
                                    {
                                        col.Item().Text($"Universidad: {tesis.Universidad}")
                                            .FontSize(10);
                                    }
                                    if (tesis.Anio.HasValue)
                                    {
                                        col.Item().Text($"Año: {tesis.Anio}")
                                            .FontSize(10).Italic();
                                    }
                                });
                            });
                        }
                    });
                }

                // Distinciones y Reconocimientos
                if (distinciones?.Any() == true)
                {
                    column.Item().PaddingTop(20).Column(distCol =>
                    {
                        distCol.Item().Text($"{GetNextSectionNumber()}. Distinciones y Reconocimientos")
                            .FontSize(14).Bold().FontColor("#2D6B3C");
                        
                        foreach (var distincion in distinciones.OrderByDescending(d => d.Anio))
                        {
                            distCol.Item().PaddingTop(10).Row(row =>
                            {
                                row.ConstantItem(15).Text("•").FontSize(12).FontColor("#2D6B3C");
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"{distincion.Nombre}")
                                        .Bold().FontSize(11);
                                    if (!string.IsNullOrEmpty(distincion.Institucion))
                                    {
                                        col.Item().Text($"Institución: {distincion.Institucion}")
                                            .FontSize(10).FontColor("#666666");
                                    }
                                    if (!string.IsNullOrEmpty(distincion.Descripcion))
                                    {
                                        col.Item().Text($"{distincion.Descripcion}")
                                            .FontSize(10).LineHeight(1.3f);
                                    }
                                    if (distincion.Anio.HasValue)
                                    {
                                        col.Item().Text($"Año: {distincion.Anio}")
                                            .FontSize(10).Italic();
                                    }
                                });
                            });
                        }
                    });
                }
            });
        }

        private string GetIconoContacto(string? tipoContacto)
        {
            return tipoContacto?.ToLower() switch
            {
                var tipo when tipo?.Contains("email") == true || tipo?.Contains("correo") == true => "✉ Email",
                var tipo when tipo?.Contains("twitter") == true => "Twitter",
                var tipo when tipo?.Contains("facebook") == true => "Facebook",
                var tipo when tipo?.Contains("linkedin") == true => "LinkedIn",
                var tipo when tipo?.Contains("instagram") == true => "Instagram",
                var tipo when tipo?.Contains("github") == true => "GitHub",
                var tipo when tipo?.Contains("web") == true || tipo?.Contains("página") == true => "⌂ Web",
                var tipo when tipo?.Contains("teléfono") == true || tipo?.Contains("telefono") == true => "☎ Teléfono",
                var tipo when tipo?.Contains("orcid") == true => "ORCID",
                var tipo when tipo?.Contains("scholar") == true => "Scholar",
                _ => "⧉ Enlace"
            };
        }

        private string GenerarFormatoAPA(PublicacionDTO publicacion)
        {
            if (publicacion == null) return string.Empty;
            
            var apa = new List<string>();

            // Autor(es) - formato apellido, iniciales
            if (!string.IsNullOrEmpty(publicacion.Autores))
            {
                apa.Add(publicacion.Autores);
            }

            // Año entre paréntesis
            if (publicacion.Anio.HasValue)
            {
                apa.Add($"({publicacion.Anio}).");
            }

            // Título en cursiva
            if (!string.IsNullOrEmpty(publicacion.Titulo))
            {
                apa.Add($"{publicacion.Titulo}.");
            }

            // Fuente/Revista
            if (!string.IsNullOrEmpty(publicacion.Fuente))
            {
                apa.Add($"{publicacion.Fuente}.");
            }

            // Enlace DOI o URL
            if (!string.IsNullOrEmpty(publicacion.Enlace))
            {
                apa.Add($"Recuperado de {publicacion.Enlace}");
            }

            return string.Join(" ", apa);
        }


        private string GetIniciales(string nombre, string? apellido)
        {
            var iniciales = "";
            if (!string.IsNullOrEmpty(nombre))
                iniciales += nombre[0];
            if (!string.IsNullOrEmpty(apellido))
                iniciales += apellido[0];
            return iniciales.ToUpper();
        }

        private DocenteDTO CrearDocenteDemo()
        {
            return new DocenteDTO
            {
                IdDocente = 1,
                NumeroEmpleado = "12345",
                NombreDocente = "Lucas Eduardo",
                PaternoDocente = "Ramos",
                MaternoDocente = "Acosta",
                NombreCategoria = "Profesor de Tiempo Completo",
                NombreCarrera = "Administración de Empresas",
                EmailInstitucional = "leramos@uabc.edu.mx",
                Extension = "1234",
                SembalzaDocente = "Egresado de Administración de Empresas en el año 1998 de la Escuela de Contabilidad y Administración. En 1999 ingresó como promotor de cursos y diplomados en el Depto. de Educación Continua, en el 2001 como inspector de trabajo en el depto. de Recursos Humanos, logrando en equipo la certificación de ISO 9000 en área de selección de personal administrativo. En el 2009 fungí como analista de compras y en 2019 como administrador de Facultad de Enología y Gastronomía.",
                NombreEscolaridad = "Maestría",
                UniversidadLicenciatura = "UABC"
            };
        }

        private List<EducacionDTO> CrearEducacionesDemo()
        {
            return new List<EducacionDTO>
            {
                new EducacionDTO
                {
                    IdEducacion = 1,
                    Nivel = "Licenciatura",
                    Titulo = "Administración de Empresas",
                    Institucion = "Escuela de Contabilidad y Administración - UABC",
                    AnioInicio = 1994,
                    AnioFin = 1998
                },
                new EducacionDTO
                {
                    IdEducacion = 2,
                    Nivel = "Maestría",
                    Titulo = "Impuestos",
                    Institucion = "UABC",
                    Especialidad = "Beneficios Fiscales a Asalariados, deducción autorizada por crédito hipotecario, personal Administrativo de una IES en Ensenada B.C. en ejercicio fiscal 2016",
                    AnioInicio = 2015,
                    AnioFin = 2017
                }
            };
        }

        private List<ExperienciaDTO> CrearExperienciasDemo()
        {
            return new List<ExperienciaDTO>
            {
                new ExperienciaDTO
                {
                    IdExperiencia = 1,
                    Puesto = "Administrador",
                    Institucion = "Facultad de Enología y Gastronomía - UABC",
                    Descripcion = "Administración general de la facultad, gestión de recursos humanos y financieros",
                    FechaInicio = new DateTime(2019, 1, 1),
                    FechaFin = null
                },
                new ExperienciaDTO
                {
                    IdExperiencia = 2,
                    Puesto = "Analista de Compras",
                    Institucion = "UABC",
                    Descripcion = "Gestión y análisis de procesos de compra institucional",
                    FechaInicio = new DateTime(2009, 1, 1),
                    FechaFin = new DateTime(2019, 1, 1)
                },
                new ExperienciaDTO
                {
                    IdExperiencia = 3,
                    Puesto = "Inspector de Trabajo",
                    Institucion = "Departamento de Recursos Humanos - UABC",
                    Descripcion = "Inspección y certificación ISO 9000 en área de selección de personal administrativo",
                    FechaInicio = new DateTime(2001, 1, 1),
                    FechaFin = new DateTime(2009, 1, 1)
                }
            };
        }

        private List<PublicacionDTO> CrearPublicacionesDemo()
        {
            return new List<PublicacionDTO>
            {
                new PublicacionDTO
                {
                    IdPublicacion = 1,
                    Titulo = "Beneficios Fiscales en Instituciones de Educación Superior",
                    TipoPublicacion = "Artículo de Investigación",
                    Autores = "Lucas Eduardo Ramos Acosta, María González López",
                    Fuente = "Revista de Administración Educativa",
                    Anio = 2020,
                    Enlace = "https://revista.uabc.edu.mx/articulo/beneficios-fiscales"
                },
                new PublicacionDTO
                {
                    IdPublicacion = 2,
                    Titulo = "Gestión de Recursos Humanos en Universidades Públicas",
                    TipoPublicacion = "Capítulo de Libro",
                    Autores = "L.E. Ramos Acosta",
                    Fuente = "Manual de Administración Universitaria, Editorial UABC",
                    Anio = 2018
                }
            };
        }

        private List<ProyectoDTO> CrearProyectosDemo()
        {
            return new List<ProyectoDTO>
            {
                new ProyectoDTO
                {
                    IdProyecto = 1,
                    Titulo = "Implementación de Sistema de Gestión de Calidad ISO 9001",
                    Rol = "Coordinador",
                    Institucion = "UABC",
                    Financiamiento = "Recursos Propios",
                    PeriodoInicio = 2008,
                    PeriodoFin = 2010
                },
                new ProyectoDTO
                {
                    IdProyecto = 2,
                    Titulo = "Modernización de Procesos Administrativos",
                    Rol = "Participante",
                    Institucion = "Facultad de Enología y Gastronomía",
                    Financiamiento = "UABC",
                    PeriodoInicio = 2020,
                    PeriodoFin = 2022
                }
            };
        }

        private List<TesisDirigidaDTO> CrearTesisDemo()
        {
            return new List<TesisDirigidaDTO>
            {
                new TesisDirigidaDTO
                {
                    IdTesis = 1,
                    Autor = "Ana María Rodríguez",
                    Titulo = "Impacto de los Beneficios Fiscales en la Administración Universitaria",
                    Nivel = "Licenciatura",
                    Universidad = "UABC",
                    Anio = 2021
                },
                new TesisDirigidaDTO
                {
                    IdTesis = 2,
                    Autor = "Carlos Alberto Mendoza",
                    Titulo = "Sistemas de Calidad en Instituciones Educativas",
                    Nivel = "Licenciatura",
                    Universidad = "UABC",
                    Anio = 2019
                }
            };
        }

        private List<DistincionDTO> CrearDistincionesDemo()
        {
            return new List<DistincionDTO>
            {
                new DistincionDTO
                {
                    IdDistincion = 1,
                    Nombre = "Reconocimiento por Años de Servicio",
                    Institucion = "UABC",
                    Descripcion = "25 años de servicio dedicado a la institución",
                    Anio = 2023
                },
                new DistincionDTO
                {
                    IdDistincion = 2,
                    Nombre = "Certificación ISO 9000",
                    Institucion = "Instituto Mexicano de Normalización",
                    Descripcion = "Certificación en sistemas de gestión de calidad",
                    Anio = 2009
                }
            };
        }

        private Dictionary<string, byte[]> CargarLogosDesdeArchivos()
        {
            Console.WriteLine("⠀⠀⠀ MÉTODO CargarLogosDesdeArchivos() EJECUTADO ⠀⠀⠀");
            var logos = new Dictionary<string, byte[]>();
            
            try
            {
                Console.WriteLine("🔍 INICIANDO CARGA DE LOGOS...");
                Console.WriteLine($"📁 Directorio actual: {Directory.GetCurrentDirectory()}");
                
                // BÚSQUEDA AGRESIVA - múltiples ubicaciones posibles
                var posiblesPaths = new[]
                {
                    // Desde la raíz del proyecto
                    Path.Combine(Directory.GetCurrentDirectory(), "Servicios", "Repositorios", "CurriculumVite", "Imagenes"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Presentacion", "wwwroot", "Imagenes"),
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagenes"),
                    
                    // Rutas relativas
                    Path.Combine("Servicios", "Repositorios", "CurriculumVite", "Imagenes"),
                    Path.Combine("Presentacion", "wwwroot", "Imagenes"),
                    Path.Combine("wwwroot", "Imagenes"),
                    Path.Combine("Imagenes"),
                    
                    // Subir un nivel (desde Presentacion)
                    Path.Combine("..", "Servicios", "Repositorios", "CurriculumVite", "Imagenes"),
                    Path.Combine("..", "Presentacion", "wwwroot", "Imagenes"),
                };

                // BUSCAR ARCHIVOS ESPECÍFICOS EN TODO EL PROYECTO
                Console.WriteLine("🔍 BÚSQUEDA EXHAUSTIVA DE ARCHIVOS...");
                
                // Buscar logo_fiad.png específicamente
                string[] fiadLocations = {
                    "./Servicios/Repositorios/CurriculumVite/Imagenes/logo_fiad.png",
                    "./Presentacion/wwwroot/Imagenes/logo_fiad.png",
                    "Servicios/Repositorios/CurriculumVite/Imagenes/logo_fiad.png",
                    "Presentacion/wwwroot/Imagenes/logo_fiad.png"
                };

                string[] uabcLocations = {
                    "./Servicios/Repositorios/CurriculumVite/Imagenes/logo_uabc.gif",
                    "./Presentacion/wwwroot/Imagenes/logo_uabc.gif",
                    "Servicios/Repositorios/CurriculumVite/Imagenes/logo_uabc.gif",
                    "Presentacion/wwwroot/Imagenes/logo_uabc.gif"
                };

                // CARGAR LOGO FIAD AGRESIVAMENTE - FORZAR DESDE UBICACIÓN CONOCIDA
                Console.WriteLine("⠀ FORZANDO CARGA DIRECTA DE FIAD...");
                bool fiadCargado = false;
                
                // FUERZA BRUTA - cargar directamente desde ubicación conocida
                var rutaForzada = "./Servicios/Repositorios/CurriculumVite/Imagenes/logo_fiad.png";
                Console.WriteLine($"🔥 FUERZA BRUTA - Probando: {rutaForzada}");
                
                if (File.Exists(rutaForzada))
                {
                    Console.WriteLine($"⠀ ARCHIVO EXISTE - CARGANDO...");
                    var fiadBytes = File.ReadAllBytes(rutaForzada);
                    logos["fiad"] = fiadBytes;
                    Console.WriteLine($"⠀ ¡¡¡FUERZA BRUTA - IMAGEN FIAD: true!!!");
                    Console.WriteLine($"📊 Tamaño FIAD: {fiadBytes.Length} bytes");
                    fiadCargado = true;
                }
                else
                {
                    Console.WriteLine($"💥 FUERZA BRUTA FALLÓ - archivo no existe");
                    
                    // Continuar con el método original
                    foreach (var fiadPath in fiadLocations)
                    {
                        Console.WriteLine($"🔍 Probando FIAD en: {fiadPath}");
                        var fullPath = Path.GetFullPath(fiadPath);
                        Console.WriteLine($"🔍 Ruta completa FIAD: {fullPath}");
                        
                        if (File.Exists(fiadPath))
                        {
                            var fiadBytes = File.ReadAllBytes(fiadPath);
                            logos["fiad"] = fiadBytes;
                            Console.WriteLine($"⠀ ¡¡¡IMAGEN FIAD: true!!!");
                            Console.WriteLine($"📊 Tamaño FIAD: {fiadBytes.Length} bytes");
                            Console.WriteLine($"📍 Ubicación FIAD: {fiadPath}");
                            fiadCargado = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($" FIAD no existe en: {fiadPath}");
                        }
                    }
                }

                if (!fiadCargado)
                {
                    Console.WriteLine($"💥 ¡¡¡IMAGEN FIAD: false!!! - NO ENCONTRADA EN NINGUNA UBICACIÓN");
                }

                // CARGAR LOGO UABC AGRESIVAMENTE - FORZAR DESDE UBICACIÓN CONOCIDA
                Console.WriteLine("⠀ FORZANDO CARGA DIRECTA DE UABC...");
                bool uabcCargado = false;
                
                // FUERZA BRUTA - cargar directamente desde ubicación conocida
                var rutaForzadaUabc = "./Servicios/Repositorios/CurriculumVite/Imagenes/logo_uabc.gif";
                Console.WriteLine($"🔥 FUERZA BRUTA UABC - Probando: {rutaForzadaUabc}");
                
                if (File.Exists(rutaForzadaUabc))
                {
                    Console.WriteLine($"⠀ ARCHIVO UABC EXISTE - CARGANDO...");
                    var uabcBytes = File.ReadAllBytes(rutaForzadaUabc);
                    logos["uabc"] = uabcBytes;
                    Console.WriteLine($"⠀ ¡¡¡FUERZA BRUTA - LOGO UABC: true!!!");
                    Console.WriteLine($"📊 Tamaño UABC: {uabcBytes.Length} bytes");
                    uabcCargado = true;
                }
                else
                {
                    Console.WriteLine($"💥 FUERZA BRUTA UABC FALLÓ - archivo no existe");
                    
                    // Continuar con el método original
                    foreach (var uabcPath in uabcLocations)
                    {
                        Console.WriteLine($"🔍 Probando UABC en: {uabcPath}");
                        
                        if (File.Exists(uabcPath))
                        {
                            var uabcBytes = File.ReadAllBytes(uabcPath);
                            logos["uabc"] = uabcBytes;
                            Console.WriteLine($"⠀ Logo UABC cargado: {uabcBytes.Length} bytes desde {uabcPath}");
                            uabcCargado = true;
                            break;
                        }
                    }
                }

                if (!uabcCargado)
                {
                    Console.WriteLine($" Logo UABC no encontrado en ninguna ubicación");
                }

                Console.WriteLine($"📊 RESUMEN FINAL: {logos.Count} logos cargados");
                foreach (var logo in logos)
                {
                    Console.WriteLine($"   ⠀ {logo.Key}: {logo.Value.Length} bytes");
                }

                // Si no se cargó nada, intentar método de emergencia
                if (logos.Count == 0)
                {
                    Console.WriteLine("⠀ MÉTODO DE EMERGENCIA - BUSCANDO EN TODO EL SISTEMA DE ARCHIVOS");
                    TentarCargarLogosEmergencia(logos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 ERROR CARGANDO LOGOS: {ex.Message}");
                Console.WriteLine($"💥 Stack trace: {ex.StackTrace}");
                
                // ÚLTIMA OPORTUNIDAD - Intentar Google Drive inmediatamente
                Console.WriteLine("⠀ ÚLTIMO RECURSO - ACTIVANDO GOOGLE DRIVE DESDE CATCH");
                try
                {
                    var logosEmergencia = CargarLogosDesdeGoogleDrive();
                    if (logosEmergencia.Count > 0)
                    {
                        Console.WriteLine($"🎉 GOOGLE DRIVE FUNCIONÓ EN EMERGENCIA: {logosEmergencia.Count} logos");
                        return logosEmergencia;
                    }
                }
                catch (Exception exDrive)
                {
                    Console.WriteLine($"💥 Google Drive también falló: {exDrive.Message}");
                }
            }
            
            Console.WriteLine($" ⠀ RETORNANDO {logos.Count} LOGOS");
            return logos;
        }

        private void TentarCargarLogosEmergencia(Dictionary<string, byte[]> logos)
        {
            try
            {
                // Buscar en directorios padre
                var currentDir = Directory.GetCurrentDirectory();
                var parentDir = Directory.GetParent(currentDir)?.FullName;
                
                if (parentDir != null)
                {
                    Console.WriteLine($"Buscando en directorio padre: {parentDir}");
                    
                    var allFiles = Directory.GetFiles(parentDir, "*logo_fiad.png", SearchOption.AllDirectories);
                    Console.WriteLine($" Archivos FIAD encontrados: {allFiles.Length}");
                    
                    foreach (var file in allFiles)
                    {
                        Console.WriteLine($"    {file}");
                        if (File.Exists(file))
                        {
                            logos["fiad"] = File.ReadAllBytes(file);
                            Console.WriteLine($" ¡¡¡EMERGENCIA - IMAGEN FIAD: true!!! desde {file}");
                            break;
                        }
                    }

                    var allUabcFiles = Directory.GetFiles(parentDir, "*logo_uabc.gif", SearchOption.AllDirectories);
                    foreach (var file in allUabcFiles)
                    {
                        if (File.Exists(file))
                        {
                            logos["uabc"] = File.ReadAllBytes(file);
                            Console.WriteLine($"⠀ EMERGENCIA - Logo UABC cargado desde {file}");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error en método de emergencia: {ex.Message}");
            }
        }

        private Dictionary<string, byte[]> CargarLogosDesdeGoogleDrive()
        {
            var logos = new Dictionary<string, byte[]>();
            
            try
            {
                Console.WriteLine("🌐 CARGANDO LOGOS DESDE GOOGLE DRIVE...");
                
                // URL de Google Drive para logo FIAD
                var fiadGoogleDriveUrl = "https://drive.google.com/file/d/1TWaMwX2RGvaiBEbatI_73Ey09p56DgUY/view?usp=sharing";
                
                // Convertir a URL de descarga directa
                var fiadDownloadUrl = ConvertirGoogleDriveUrlADescarga(fiadGoogleDriveUrl);
                Console.WriteLine($"🔗 URL descarga FIAD: {fiadDownloadUrl}");
                
                // Descargar logo FIAD
                var fiadBytes = DescargarImagenDesdeDrive(fiadGoogleDriveUrl);
                if (fiadBytes != null && fiadBytes.Length > 0)
                {
                    logos["fiad"] = fiadBytes;
                    Console.WriteLine($"⠀ ¡¡¡GOOGLE DRIVE - IMAGEN FIAD: true!!!");
                    Console.WriteLine($"📊 Tamaño FIAD desde Drive: {fiadBytes.Length} bytes");
                }
                else
                {
                    Console.WriteLine($"💥 No se pudo cargar FIAD desde Google Drive");
                }

                // Para UABC, usar una imagen por defecto o crear una
                var uabcBytes = CrearLogoUABCPorDefecto();
                if (uabcBytes != null)
                {
                    logos["uabc"] = uabcBytes;
                    Console.WriteLine($"⠀ Logo UABC creado por defecto: {uabcBytes.Length} bytes");
                }

                Console.WriteLine($"📊 GOOGLE DRIVE - Logos cargados: {logos.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error cargando desde Google Drive: {ex.Message}");
            }
            
            return logos;
        }

        private string ConvertirGoogleDriveUrlADescarga(string googleDriveUrl)
        {
            try
            {
                // Extraer el ID del archivo de la URL
                if (googleDriveUrl.Contains("/d/"))
                {
                    var fileId = googleDriveUrl.Split("/d/")[1].Split("/")[0];
                    return $"https://drive.google.com/uc?export=download&id={fileId}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error convirtiendo URL: {ex.Message}");
            }
            
            return googleDriveUrl;
        }

        private byte[]? CrearLogoUABCPorDefecto()
        {
            try
            {
                // Crear una imagen simple para UABC como fallback
                // Por ahora, devolver null para usar texto
                return null;
            }
            catch
            {
                return null;
            }
        }

        private byte[]? DescargarImagenDesdeDrive(string driveUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(driveUrl))
                    return null;

                // Extraer el ID del archivo de la URL de Google Drive
                string fileId;
                if (driveUrl.Contains("/d/"))
                {
                    fileId = driveUrl.Split("/d/")[1].Split("/")[0];
                }
                else if (driveUrl.Contains("id="))
                {
                    fileId = driveUrl.Split("id=")[1].Split("&")[0];
                }
                else
                {
                    return null;
                }

                // Construir la URL de descarga directa
                var downloadUrl = $"https://drive.google.com/uc?export=download&id={fileId}";

                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                
                // Agregar headers para simular un navegador
                httpClient.DefaultRequestHeaders.Add("User-Agent", 
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                var response = httpClient.GetAsync(downloadUrl).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                    
                    // Verificar que sea una imagen válida (al menos 1KB)
                    if (imageBytes.Length > 1024)
                    {
                        return imageBytes;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                // Si hay cualquier error, devolver null para usar fallback
                return null;
            }
        }
    }
} 