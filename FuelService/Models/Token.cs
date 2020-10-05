using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelService.Models
{
    public class Token
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expired_in { get; set; }

        public string access_token { get; set; }

        public int ext_expired_in { get; set; }
    }
}
