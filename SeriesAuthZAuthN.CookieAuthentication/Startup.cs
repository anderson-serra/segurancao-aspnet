using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SeriesAuthZAuthN.CookieAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // TODO: Default Schema
            // Referente a autenticação
            services.AddAuthentication(Configuration["DefaultSchema"])
                    .AddCookie(Configuration["DefaultSchema"], opt => opt.Cookie.Name = "SeriesAuth");

            // Referente a autorização
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", p => { p.RequireRole("SecretRole"); });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            // TODO: UseAuthentication() deve vir antes do UseAuthorization()
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
