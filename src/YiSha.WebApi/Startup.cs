using System.Text.Json;
using System.Text.Json.Serialization;
using Furion;
using YiSha.WebApi.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YiSha.Util;
using YiSha.WebApi.ServiceExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace YiSha.WebApi
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsAccessor();

            services.AddJwt(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(cfg => cfg.SlidingExpiration = true);

            services.AddControllers().AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                })
                .AddNewtonsoftJson(options =>
                {
                    // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddInjectWithUnifyResult()
                .AddFriendlyException() // 注册友好异常服务
                .AddUnifyResult<ExResultProvider>();
            
            // 注册拦截器
            services.AddScoped<AuthorizeFilterAttribute>();
            // // if (env.IsDevelopment())
            // // {
            // //     services.AddControllersWithViews().AddRazorRuntimeCompilation();
            // // }
            // // else
            // // {
            services.AddControllersWithViews();
            // }
            services.AddAutoMapperSetup();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }
    }
}
