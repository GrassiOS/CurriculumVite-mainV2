using AutoMapper;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.PerfilesDTO.CurriculumVite
{
    public class DocenteProfile : Profile
    {
        public DocenteProfile()
        {
            CreateMap<DocenteDTO, E_Docente>()
                .ForMember(dest => dest.Sexo, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoCivil, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.Nombramiento, opt => opt.Ignore())
                .ForMember(dest => dest.Escolaridad, opt => opt.Ignore())
                .ForMember(dest => dest.NivelSNI, opt => opt.Ignore())
                .ForMember(dest => dest.PRODEP, opt => opt.Ignore())
                .ForMember(dest => dest.Carrera, opt => opt.Ignore())
                .ForMember(dest => dest.CuerpoAcademico, opt => opt.Ignore())
                .ForMember(dest => dest.Contactos, opt => opt.Ignore())
                .ForMember(dest => dest.Educaciones, opt => opt.Ignore())
                .ForMember(dest => dest.Experiencias, opt => opt.Ignore())
                .ForMember(dest => dest.Publicaciones, opt => opt.Ignore())
                .ForMember(dest => dest.Proyectos, opt => opt.Ignore())
                .ForMember(dest => dest.TesisDirigidas, opt => opt.Ignore())
                .ForMember(dest => dest.Distinciones, opt => opt.Ignore())
                .ForMember(dest => dest.Documentos, opt => opt.Ignore());

            CreateMap<E_Docente, DocenteDTO>()
                .ForMember(dest => dest.NombreSexo, opt => opt.MapFrom(src => src.Sexo != null ? src.Sexo.Sexo : null))
                .ForMember(dest => dest.NombreEstadoCivil, opt => opt.MapFrom(src => src.EstadoCivil != null ? src.EstadoCivil.EstadoCivil : null))
                .ForMember(dest => dest.NombreCategoria, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.NombreCategoria : null))
                .ForMember(dest => dest.NombreNombramiento, opt => opt.MapFrom(src => src.Nombramiento != null ? src.Nombramiento.Nombramiento : null))
                .ForMember(dest => dest.NombreEscolaridad, opt => opt.MapFrom(src => src.Escolaridad != null ? src.Escolaridad.NombreEscolaridad : null))
                .ForMember(dest => dest.NombreNivelSNI, opt => opt.MapFrom(src => src.NivelSNI != null ? src.NivelSNI.NivelSNI : null))
                .ForMember(dest => dest.NombrePRODEP, opt => opt.MapFrom(src => src.PRODEP != null ? src.PRODEP.TienePRODEP : null))
                .ForMember(dest => dest.NombreCarrera, opt => opt.MapFrom(src => src.Carrera != null ? src.Carrera.NombreCarrera : null))
                .ForMember(dest => dest.AliasCarrera, opt => opt.MapFrom(src => src.Carrera != null ? src.Carrera.AliasCarrera : null))
                .ForMember(dest => dest.NombreCuerpoAcademico, opt => opt.MapFrom(src => src.CuerpoAcademico != null ? src.CuerpoAcademico.Nombre : null))
                .ForMember(dest => dest.CantidadEducaciones, opt => opt.MapFrom(src => src.Educaciones != null ? src.Educaciones.Count : 0))
                .ForMember(dest => dest.CantidadExperiencias, opt => opt.MapFrom(src => src.Experiencias != null ? src.Experiencias.Count : 0))
                .ForMember(dest => dest.CantidadPublicaciones, opt => opt.MapFrom(src => src.Publicaciones != null ? src.Publicaciones.Count : 0))
                .ForMember(dest => dest.CantidadProyectos, opt => opt.MapFrom(src => src.Proyectos != null ? src.Proyectos.Count : 0))
                .ForMember(dest => dest.CantidadTesisDirigidas, opt => opt.MapFrom(src => src.TesisDirigidas != null ? src.TesisDirigidas.Count : 0))
                .ForMember(dest => dest.CantidadDistinciones, opt => opt.MapFrom(src => src.Distinciones != null ? src.Distinciones.Count : 0))
                .ForMember(dest => dest.CantidadDocumentos, opt => opt.MapFrom(src => src.Documentos != null ? src.Documentos.Count : 0));
        }
    }
} 