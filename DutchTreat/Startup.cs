using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using System.Reflection;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat
{
  public class Startup
  {
        public readonly IConfiguration Config;

        public Startup(IConfiguration _config)
        {
            Config = _config;
        }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
            services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlServer(Config.GetConnectionString("DutchConectionString"));
            }) ;
            services.AddIdentity<StoreUser, IdentityRole>(
            cfg =>
             {
                 cfg.User.RequireUniqueEmail=true;
                 cfg.Password.RequiredLength = 3;

             })
                .AddEntityFrameworkStores<DutchContext>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();
            services.AddScoped<IDutchRepository, DutchRepository>();
            services.AddControllersWithViews();

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            //    .AddJsonOptions(opt => opt.Json);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
            // app.UseDefaultFiles();   
      if (env.IsEnvironment("Development"))
      {
          app.UseDeveloperExceptionPage();
      }
      else
      {

           app.UseExceptionHandler("/error");
          //Responsive Page for User
      }
      app.UseStaticFiles();
      app.UseNodeModules();
      app.UseAuthentication();     
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(cfg =>
      {
          cfg.MapControllerRoute("FallBack", "{controller}/{action}/{id?}",
          new { controller = "app", action = "Index" });
      });
            
    }
  }
}
