using System;
using System.Collections.Generic;

namespace DotNetCoreSamples.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; } //ICollection order ve orderitem arasındaki ilişkiyi yaratacak.
        public StoreUser User { get; set; }
    }
}
