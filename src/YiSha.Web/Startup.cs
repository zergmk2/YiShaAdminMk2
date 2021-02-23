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

            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });

            GlobalContext.ServiceProvider = app.ApplicationServices;

            // mqttClient.ConnectMqttServerAsync().GetAwaiter().GetResult();
        }
    }
}
