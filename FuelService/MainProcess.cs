using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json.Linq;
using FuelService.Models;

namespace FuelService
{
    public sealed class MainProcess
    {

        public static void initiateProcess(bool fulltrans)
        {
            
        }
        public static JObject RunData()
        {
            return null;
            //response.EnsureSuccessStatusCode();

            //string content = await response.Content.ReadAsStringAsync();
            // return await Task.Run(() => JObject.Parse(content));
        }
        public void RunSiteService()
        {

            var a = new Token();
            



        }
    }


}
