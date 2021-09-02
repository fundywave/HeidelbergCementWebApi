using HeidelbergCement.Data.DTO;
using HeidelbergCement.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HeidelbergCement.Service.Interface;
using HeidelbergCement.Service.Provider;
using HeidelbergCement.Service.Repositories;
using HeidelbergCement.Service.Service;
using HeidelbergCement.WebApi.Security.Authentication;

namespace HeidelbergCement
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
            services.AddControllers();
            services.AddSingleton<IUserRepository<User>, UserRepository>();
            services.AddTransient<IDataProvider<ResponseRecord>,DataProvider<ResponseRecord>>();
            services.AddTransient<IAirTableDataProvider, AirTableDataProvider>();
            services.AddScoped<IUserService<User>, UserService>();

            //Basic Authentication
            services.AddAuthentication("Basic").AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("Basic", null);
            services.AddTransient<IAuthenticationHandler, BasicAuthenticationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
