using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        public SqlMoney Amount { get; set; }

    }
}
