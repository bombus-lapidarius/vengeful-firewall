using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForcefulFi.WebApi
{
    public class Startup // the usual C# / Java boilerplate code wrapper
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // make our custom controllers known to the application
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // the http request pipeline constitutes hidden state

            // for debugging
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // add routing capabilities to our http request pipeline
            app.UseRouting();

            // add processing endpoints to our http request pipeline
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                
                // map routes defined in attributes to their controllers
                endpoints.MapControllers();
            });
        }
    }
} // namespace ForcefulFi.WebApi
