using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using FuelService.Helpers;
using FuelService;
namespace FuelService
{
    public partial class FuelService : ServiceBase
    {
        internal Timer Timer = new Timer();
        public FuelService()
        {
            this.ServiceName = ConfigurationManager.AppSettings["serviceName"].ToString(CultureInfo.InvariantCulture);
           
        }

        protected override void OnStart(string[] args)
        {

            var data = MainProcess.RunData();

        }

        protected override void OnStop()
        {
        }

    }
}
