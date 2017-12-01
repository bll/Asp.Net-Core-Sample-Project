using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreSamples.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        //Identity ekleyip dbContext yerine IdentityDbContext kullandıktan sonra -> initializer , 'dotnet ef' çağrısı sırasında başlatılıyor olmasından. An error occurred while calling method 'BuildWebHost' on class 'Program' hatası alıyordum
        // araştırmam sonucu IDesignTimeDbContextFactory uygulamasını ekleyerek bu sorunu giderebildim. 
        public MyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", false)
                    .Build();
            var connectionString = configuration.GetConnectionString("MyDbConnectionString");
            builder.UseSqlServer(connectionString);
            return new MyDbContext(builder.Options);
        }
    }
}
