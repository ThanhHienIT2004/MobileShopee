using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShopee.Models
{
    public class Transaction
    {
        public string TransId { get; set; }
        public string ModelId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
