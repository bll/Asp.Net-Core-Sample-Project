using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNetCoreSamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //bir web hosting kuruyor ve 80 portundan gelen istekleri dinliyor
            BuildWebHost(args).Run();
        }

        //burası webhost için defaultBuilder ve hangi sınıfın kullanacağını söyler
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
                .UseStartup<Startup>() // web istekleri nasıl dinleyeceğimizi ayarlamak için startup.cs sınıfını kullanır
                .Build();

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //default konfigrasyon ayarlarını sil
            builder.Sources.Clear();

            //config dosyası değişirse yapılandırmanın yeniden yüklenip yüklenmeyeceği seçeneği 
            builder.AddJsonFile("config.json", false, true)
                //.AddXmlFile("config.xml") -> config dosyamızı bir xml den de alabilirdik veya bir azure key file de olabilirdi
                .AddEnvironmentVariables();

        }
    }
}
