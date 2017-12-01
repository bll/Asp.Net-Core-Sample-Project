using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DotNetCoreSamples.Services;
using DotNetCoreSamples.Data;
using DotNetCoreSamples.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DotNetCoreSamples
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }


        // Bu yöntem çalışma zamanı tarafından çağrılır. Kapta hizmetler eklemek için bu yöntemi kullanın.
        // Uygulamanızı yapılandırma hakkında daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkID=398940 adresini ziyaret edin.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MyDbContext>(); // burada identity verisini farklı bir veritabanıdan tutmak istersek o context i verebiliriz


            services.AddDbContext<MyDbContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("MyDbConnectionString"));
            });

            services.AddAutoMapper();

            services.AddTransient<IMailService, NullMailService>(); //IMailService sürekli kullanılan bir servis olmayacağı için tanımladım AddTransient tanımladım. Aksi durumda AddSingleton tanımlanabilirdi.

            //seed data
            services.AddTransient<MyDbSeeder>();

            services.AddScoped<IRepository, MyRepository>();

            services.AddMvc(
                //opt =>
                //{
                //    if (_env.IsProduction())
                //    {
                //        opt.Filters.Add(new RequireHttpsAttribute());
                //    }
                //}
                )

                // Api içinde ilişkili tablolarda verileri serileştiremeyebiliyor (kendi kendini referanslayan bir koleksiyon olduğundan dolayı).  
                //Bazı tarayıcılarda  çalışabilir (chrome) ama postmanda çalışmadı garantiye almak için bu durumu devre dışı bırakıyorum
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
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

            app.UseAuthentication(); // bu middleware mvc den önce gelmeli aksi taktirde mvc kimlik doğrulamasını kullanamaz

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default", "{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });
            });


            // seed dataları sadece geliştirme modunda dahil edilsin
            if (env.IsDevelopment())
            {
                //sadece using bloğu süresince nesneyi oluştursun
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<MyDbSeeder>();//ServiceProvider hizmet örneği oluşturan bir nesnedir
                    seeder.Seed().Wait();
                    // FindByEmailAsync için metodu asenkron yaptım çünkü identity tüm find metodları asenkron, burada senkronize edebilmek için wait kullandım.
                    // Seed i arayacak ve tamamlanıncaya kadar bekleyecek uygulama birkez çalıştırılacağından bir sıkıntı yaratmayacaktır
                    // tüm Configure sınıfını asenkrona çevirmememin sebebi: Bu sürümde asenkron bu yapı için iyi çalışmıyordu
                }
            }
        }
    }
}
