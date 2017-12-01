using DotNetCoreSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotNetCoreSamples.Data
{
    public class MyDbContext : IdentityDbContext<StoreUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        //Order içinde ICollection ile orderItems ilişkisi verdiğimiz için OrderItem tablosunu otomatik oluşturacaktır. İstersem buraya yazmayabilirim

    }
}
