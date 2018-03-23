using EKE.Data.Entities.Vandortabor;
using EKE.Data.Infrastructure;
using EKE.Data.Repository;
using EKE.Data.Repository.Base;
using EKE.Service.Services.Vt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EKE_WebApi
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
            // Add framework services.
            services.AddDbContext<EKE.Data.BaseDbContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("EKEConnectionString")));

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            //Add Framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IVtServices, VtServices>();

            services.AddTransient<IEntityBaseRepository<VtAccomodationType>, EntityBaseRepository<VtAccomodationType>>();
            services.AddTransient<IEntityBaseRepository<VtMembership>, EntityBaseRepository<VtMembership>>();

            //Add Services
            services.AddCors();
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.WithOrigins("http://localhost:4200/", "http://localhost:49223/").AllowAnyMethod());
            app.UseMvc();
        }
    }
}
