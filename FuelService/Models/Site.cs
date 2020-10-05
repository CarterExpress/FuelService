using System.Collections.Generic;

namespace FuelService.Models
{

        public class Site
        {
            public int SiteId { get; set; }
            public List<FuelPrice2> FuelPrices { get; set; }
        }


}
