using System.Linq;
using FluentValidation.AspNetCore;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas.Validators;
using GerenciadorDeCervejas.Mensageria.Escutas;
using GerenciadorDeCervejas.Mensageria.Infra;
using GerenciadorDeCervejas.Repositorio.Contexto;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scrutor;

namespace GerenciadorDeCervejas.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.Configure<ConfiguracoesRabbitMq>(Configuration.GetSection("RabbitMq"));

            services.AddControllers(opts => opts.EnableEndpointRouting = false)
                    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null)
                    .AddNewtonsoftJson(opts =>
                    {
                        opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    })
                    .AddFluentValidation(opts =>
                    {
                        opts.RegisterValidatorsFromAssemblyContaining(typeof(CervejaValidator));
                    });

            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<ContextoBanco>(o => o.UseNpgsql(Configuration.GetConnectionString("Conexao")));

            services.Scan(scan =>
                scan.FromApplicationDependencies(x => x.FullName.Contains("GerenciadorDeCervejas"))
                    .AddClasses(x => x.NotInNamespaceOf<EscutaEventoNotificarAlteracaoCerveja>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
                    
            services.AddOData();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            services.AddHostedService<EscutaEventoNotificarAlteracaoCerveja>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Serviços do gerenciador de cervejas"));

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) 
                .AllowCredentials());

            app.UseAuthorization();

            app.UseODataBatching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Select().Filter().OrderBy().Count().MaxTop(50);
                endpoints.MapODataRoute("odata", "api", GetEdmModel(), new DefaultODataBatchHandler());
            });
        }

        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Cerveja>("Cervejas");

            return odataBuilder.GetEdmModel();
        }
    }
}
