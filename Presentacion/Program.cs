using Datos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentacion.Components;
using Presentacion.Components.Account;
using Presentacion.Data;
using Presentacion.Helper;
using Datos.IRepositorios.PlnesDeEstudio;
using Datos.Repositorios.PlanesDeEstudio;
using Entidades.PerfilesDTO.PlanesDeEstudio;
using Negocios.Repositorios.PlanesDeEstudio;
using Servicios.Repositorios.PlanesDeEstudio;

// CurriculumVite
using Datos.IRepositorios.CurriculumVite;
using Datos.Repositorios.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Servicios.Repositorios.CurriculumVite;
using Entidades.PerfilesDTO.CurriculumVite;
using Negocios.Repositorios.CurriculumVite;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();


/*********************************************************************************************************
*                     Servicios agregados para el proyecto                                               *
*********************************************************************************************************/


/*
 *  Trace: Registra eventos muy detallados y técnicos. Se utiliza para diagnóstico.
 *  Deatallado: Este nivel rara vez se usa en produccción debido a la gran cantidad de datos generados.
 */

//CONTEXTOS

// Configuración de la conexion
var connectionString = builder.Configuration.GetConnectionString("ConexionBD")
  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Contexto para la gestion de usuarios creado para las cuentas individuales
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Contexto de base de datos (ContextoBD) configurada en la capa de datos
builder.Services.AddDbContext<ContextoBD>(options => options.UseSqlServer(connectionString));

// Registro del repositorios de carreras
builder.Services.AddScoped<ICarreraRepositorio, CarreraRepositorio>();
builder.Services.AddScoped<IMateriaRepositorio, MateriaRepositorio>();
builder.Services.AddScoped<IPlanEstudioRepositorio, PlanEstudioRepositorio>();

// Inyección de dependencias para la capa de datos y servicios
builder.Services.AddScoped<CarreraRepositorio>();
builder.Services.AddScoped<CarreraNegocios>();
builder.Services.AddScoped<CarreraServicios>();

builder.Services.AddScoped<MateriaRepositorio>();
builder.Services.AddScoped<MateriaNegocios>();
builder.Services.AddScoped<MateriaServicios>();

builder.Services.AddScoped<PlanEstudioRepositorio>();
builder.Services.AddScoped<PlanEstudioNegocios>();
builder.Services.AddScoped<PlanEstudioServicios>();


// —— Inyección de dependencias para CurriculumVite ——

// Docente CRUD completo
builder.Services.AddScoped<IDocenteRepositorio, DocenteRepositorio>();
builder.Services.AddScoped<DocenteNegocios>();
builder.Services.AddScoped<ISDocenteRepositorio, DocenteServicios>();

// Otras entidades de CurriculumVite
builder.Services.AddScoped<IRepositorioDistincion, DistincionRepositorio>();
builder.Services.AddScoped<IRepositorioExperiencia, ExperienciaRepositorio>();
builder.Services.AddScoped<IRepositorioTipoContacto, TipoContactoRepositorio>();
builder.Services.AddScoped<IRepositorioContactoDocente, ContactoDocenteRepositorio>();
builder.Services.AddScoped<IRepositorioProyecto, ProyectoRepositorio>();
builder.Services.AddScoped<Datos.IRepositorios.CurriculumVite.IRepositorioEducacion, EducacionRepositorio>();
builder.Services.AddScoped<IRepositorioPublicacion, PublicacionRepositorio>();
builder.Services.AddScoped<IRepositorioTesisDirigida, TesisDirigidaRepositorio>();
builder.Services.AddScoped<IRepositorioDocumento, DocumentoRepositorio>();

builder.Services.AddScoped<ISRepositorioDistincion, DistincionServicios>();
builder.Services.AddScoped<ISRepositorioExperiencia, ExperienciaServicios>();
builder.Services.AddScoped<ISRepositorioTipoContacto, TipoContactoServicios>();
builder.Services.AddScoped<ISRepositorioContactoDocente, ContactoDocenteServicios>();
builder.Services.AddScoped<ISRepositorioProyecto, ProyectoServicios>();
builder.Services.AddScoped<ISRepositorioEducacion, EducacionServicios>();
builder.Services.AddScoped<ISRepositorioPublicacion, PublicacionServicios>();
builder.Services.AddScoped<ISRepositorioTesisDirigida, TesisDirigidaServicios>();
builder.Services.AddScoped<ISRepositorioDocumento, DocumentoServicios>();
builder.Services.AddScoped<ISRepositorioPDFGenerator, SRepositorioPDFGenerator>();


// Registro del DocumentRepository para subida de archivos
builder.Services.AddScoped<Servicios.IRepositorios.IDocumentRepository, Servicios.Repositorios.DocumentRepository>();

// Inyeccion de dependecia para SweeetAlert
builder.Services.AddScoped<SweetAlertServicios>();

//Este servicio es importante para detallar los erreres en tiempo de ejecución
builder.Services.AddServerSideBlazor(options => {
    options.DetailedErrors = true; // Habilita errores detallados
});

// En ConfigureServices - Incluir todos los perfiles de AutoMapper
builder.Services.AddAutoMapper(typeof(CarreraProfile), typeof(DocenteProfile), typeof(ContactoDocenteProfile), typeof(TipoContactoProfile), typeof(EducacionProfile));

//Servicio para Quill Editor
builder.Services.AddHttpClient(); // Agrega este registro


/***********************************************************************************************************/




builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
