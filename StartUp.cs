using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AuthKalumManagement.DbContext;
using AuthKalumManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthKalumManagement
{
    public class StartUp
    {
        private readonly IConfiguration Configurations; // esta propiedad es creada para poder conectarse a la bd

        public StartUp(IConfiguration _Configurations)
        {
            this.Configurations = _Configurations;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthKalumManagementContext>(options =>{
                options.UseSqlServer(this.Configurations.GetConnectionString("AuthKalumManagementConnection"));
            });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthKalumManagementContext>()
                .AddDefaultTokenProviders();
            services.AddAutoMapper(typeof(StartUp));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
                //app.UseHttpsRedirection(); este metodo su funcion es redireccionar todo por el protocolo https pero ahorita no se usara ya que no es un avente de prodction es una demo esto se usa en ambientes productivos ¿cetificado autenticado
                app.UseRouting();
                app.UseCors();
                app.UseAuthentication();
                app.UseEndpoints(Endpoints => {
                    Endpoints.MapControllers();
                });


        }
    }
}