using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShopee.Models
{
    public class Sale
    {
        public string SId { get; set; }
        public string IMEINO { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string CustId { get; set; }
    }
}
