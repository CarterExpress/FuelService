using System;

namespace FuelService.Models
{

        public class FuelPrice2
        {
            public int SiteId { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string FuelType { get; set; }
            public DateTime LastPriceChangeDateTime { get; set; }
            public DateTime LastCheckInDateTime { get; set; }
            public double CashPrice { get; set; }
            public double? CreditPrice { get; set; }
        }
}
