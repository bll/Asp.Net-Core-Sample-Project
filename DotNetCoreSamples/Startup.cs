using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DotNetCoreSamples.Services;

namespace DotNetCoreSamples
{
    public class Startup
    {
        // Bu yöntem çalışma zamanı tarafından çağrılır. Kapta hizmetler eklemek için bu yöntemi kullanın.
        // Uygulamanızı yapılandırma hakkında daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkID=398940 adresini ziyaret edin.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMailService, NullMailService>(); //AddTransient sürekli kullanılan bir servis olmayacağı için tanımladım. Aksi durumda AddSingleton tanımlanabilirdi.
            services.AddMvc();
        }

        // Bu yöntem çalışma zamanı tarafından çağrılır. HTTP isteği bağlantı hattını yapılandırmak için bu yöntemi kullanın.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // hatanın buraya düşmesi için proje ayarlarından ASPNETCORE_ENVIRONMENT Development modundan Product a çekmem gerekir.
                app.UseExceptionHandler("/error");
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            // app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default", "{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });
            });
        }
    }
}
