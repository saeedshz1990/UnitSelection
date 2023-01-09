using Autofac;
using Microsoft.OpenApi.Models;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.Terms;
using UnitSelection.Services.Terms;

namespace UnitSelection.RestApi;

public class Start
{
    public Start(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", 
                new OpenApiInfo { Title = "UnitSelection.RestApi", Version = "v1" });
        });
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<EFDataContext>()
            .WithParameter("connectionString", Configuration["ConnectionString"])
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(EFTermRepository).Assembly)
            .AssignableTo<Repository>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(TermAppService).Assembly)
            .AssignableTo<Service>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterType<EFUnitOfWork>()
            .As<UnitOfWork>()
            .InstancePerLifetimeScope();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => 
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "UnitSelection.RestApi v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}