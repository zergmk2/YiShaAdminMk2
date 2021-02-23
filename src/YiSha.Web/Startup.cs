using System.IO;
using YiSha.Mqtt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<SystemConfig>();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MqttClientCenter mqttClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("login.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseSession();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Server-Ext", "FX-EWB-Compatible");
                    context.Context.Response.Headers.Add("X-FX-EWB-Version", "4.0");
                },
            });

            // var resource = Path.Combine(env.ContentRootPath, "Resource");
            // FileHelper.CreateDirectory(resource);
            // app.UseStaticFiles(new StaticFileOptions
            // {
            //     OnPrepareResponse = context =>
            //     {
            //         context.Context.Response.Headers.Add("Server-Ext", "FX-EWB-Compatible");
            //         context.Context.Response.Headers.Add("X-FX-EWB-Version", "4.0");
            //     },
            //     RequestPath = "/Resource",
            //     FileProvider = new PhysicalFileProvider(resource)
            // });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            GlobalContext.ServiceProvider = app.ApplicationServices;

            // mqttClient.ConnectMqttServerAsync().GetAwaiter().GetResult();
        }
    }
}
