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

    public class SaleReports
    {
        public string SaleId { get; set; }
        public string CompanyName { get; set; }
        public string ModelNumber { get; set; }
        public string IMEINO { get; set; }
        public string Price { get; set; }
    }
}
