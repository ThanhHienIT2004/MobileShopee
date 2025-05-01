using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShopee.Models
{
    public class Mobile
    {
        public string ModelId { get; set; }
        public string IMEINO { get; set; }
        public string Status { get; set; }
        public DateTime? Warranty { get; set; }
        public SqlMoney Price { get; set; }
    }
}
