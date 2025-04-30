using System;
using System.Data.SqlTypes;

namespace MobileShopee.Models {
    public class Mobile {
        public string ModelId { get; set; }
        public string IMEINo { get; set; }
        public string Status { get; set; 
        } public DateTime WarrantyDate { get; set; }
        public SqlMoney Price { get; set; } 
    } 
}
