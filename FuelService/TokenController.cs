using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FuelService.Helpers;
using FuelService.Models;
using Newtonsoft.Json;
using System.IO;
namespace FuelService
{
    public partial class TokenService
    {
    
        // POST api/values
        public Task RunService()
        {

            //THIS SHOULD LATER COME FROM WHATEVER INPUT WE ARE GETTING QUOTES FROM TODO:
            DateTime bidForDate = DateTime.Now;

           // SetLovePrices();

            GetCSVQuotes();
            //-----------------------------------------------------------------------------
            //This code is only needed after we have an input type to gather vendor quotes
            //InsertIntoVendorBids(1100, 2.60, bidForDate);
            //-----------------------------------------------------------------------------


            //-----------------------------------------------------------------------------
            //This code is used to send the confirmation email to the folks that get notified (the email users are set in the App.config along with the email content)
            //SendEmail();
            //-----------------------------------------------------------------------------


            //--------------------------------------------------------------------------
            //This code is only needed IF WE USE THE FUEL PURCHASING email to gather quotes
            //RunEmailInputCheck();
            //---------------------------------------------------------------------------


            return null;
        }

        private List<string> GetCSVFiles()
        {
            var fileList = new List<string>();
            int count = 0;

            string[] dirs = Directory.GetDirectories(@"\\\\\naus16alpha1\d$\Carter Websites\FuelService\Quotes\", "*PREBATCH*", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                var csvFiles = Directory.GetFiles(dir, "*.csv");
                foreach (var file in csvFiles)
                {
                    if (File.GetCreationTime(file) > DateTime.Now.Date)
                    {
                        fileList.Add(file.ToString());
                        count++;
                    }
                }
            }
            return fileList;
        }


        private void GetCSVQuotes()
        {
           // var files = Directory.GetFiles("\\naus16alpha1\d$\Carter Websites\FuelService\Quotes").Where(x => x.);
            var fileNames = GetCSVFiles();

            foreach (var file in fileNames)
                {
                    

                }

            string path = "C:/Users/jreed/OneDrive - Carter Logistics/Desktop/testcsv.csv";

            string[] lines = System.IO.File.ReadAllLines(path);
            int row = 1;

            quotePrice priceForQuote = new quotePrice();
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                foreach (string column in columns)
                {
                    if(row == 1)
                    {
                        priceForQuote.dateOfQuote = DateTime.Parse(column);
                    }
                    else
                    {
                        priceForQuote.priceQuote = double.Parse(column);
                    }
                    row++;
                }
            }
            var a = priceForQuote;
            InsertIntoVendorBids(1100, priceForQuote.priceQuote, priceForQuote.dateOfQuote);
        }

        private class quotePrice
        {
            public DateTime dateOfQuote {get;set;}
            public double priceQuote { get; set; }
        }
        private void RunEmailInputCheck()
        {
            var tokenString = TokenInfo();
            var emails = GetEmail(tokenString.Result.ToString());

            string emailString = "";

            foreach (var item in emails.Result.value)
            {
                emailString += item.body.content.ToString();
            }

        }


        //APIs for Love's with specific ones to grab Pendleton and Marion. The long and lat make that station the number 1 pick
        internal async Task<bool> SetLovePrices()
        {

            string pendletonURL = "https://www.loves.com/api/sitecore/StoreSearch/SearchStoresWithDetail?pageNumber=0&top=1&lat=39.992157&lng=-85.845121";
            string marionURL = "https://www.loves.com/api/sitecore/StoreSearch/SearchStoresWithDetail?pageNumber=0&top=1&lat=40.554335&lng=-85.548979";

            double? loveListMarion = await GetLocationPrices(marionURL);
            double? loveListPendleton = await GetLocationPrices(pendletonURL);
            //THIS SHOULD LATER COME FROM WHATEVER INPUT WE ARE GETTING QUOTES FROM TODO:
            DateTime bidForDate = DateTime.Now;

            var priceSaved = SaveOwnerOpsPrice(loveListMarion, loveListPendleton, bidForDate);



            return true;

        }

        //Method to save info
        internal bool SaveOwnerOpsPrice(double? priceMarion, double? pricePendleton, DateTime BidForDate)
        {

            OwnerPriceInfo OP = new OwnerPriceInfo();

            if (priceMarion == null || pricePendleton == null)
            {
                return false;
            }

            if (priceMarion != null)
            {
                OP.North69 = priceMarion;

            }
            if (pricePendleton != null)
            {
                OP.South69 = pricePendleton;
            }
            //THIS SHOULD LATER COME FROM WHATEVER INPUT WE ARE GETTING QUOTES FROM TODO:
            OP.BidForDate = BidForDate;

            InsertIntoOwnerFuel(OP);

            return true;

        }


        public static int InsertIntoOwnerFuel(OwnerPriceInfo OP)
        {
            try
            {
                using (IDbConnection db =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["FuelPurchaseSQL"].ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@priceMarion", OP.North69);
                    parameters.Add("@pricePendleton", OP.South69);
                    parameters.Add("@BidForDate", OP.BidForDate);

                    var result = db.Execute("up_Insert_OwnerOpsFuelPricing", param: parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int InsertIntoVendorBids(int FuelVendorID, double Price, DateTime BidForDate)
        {
            try
            {
                using (IDbConnection db =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["FuelPurchaseSQL"].ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@FuelVendorID", FuelVendorID);
                    parameters.Add("@FuelPrice", Price);
                    parameters.Add("@BidForDate", BidForDate);


                    var result = db.Execute("up_Insert_VendorBids", param: parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //Method to get the Love's credit price for Diesel for the day from their API
        internal async Task<double?> GetLocationPrices(string url)
        {
            var loveList = await getLovesPrices(url);
            var LovePriceList = loveList.FirstOrDefault().FuelPrices.Where(x => x.ProductName == "DIESEL").FirstOrDefault();

            return LovePriceList.CreditPrice != null ? LovePriceList.CreditPrice : 0;
        }


        //Token information that we need to get permission to get the actual email token we need to grab the email for fuelpurchasing
        public async Task<string> TokenInfo()
        {
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent("password"), "grant_type");
                content.Add(new StringContent("5109ce58-7573-4cab-b38f-14f0375b7d44"), "client_id");
                content.Add(new StringContent("I_8l7L8tbNtvRYgn43tar-~euz8q27-s~v"), "client_secret");
                content.Add(new StringContent("fuelpurchasing@carter-express.com"), "userName");
                content.Add(new StringContent("Carter4020!"), "password");
                content.Add(new StringContent("https://graph.microsoft.com/.default"), "scope");



                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/b98107dc-2506-4548-960e-43d87467715c/oauth2/v2.0/token");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                requestMessage.Headers.Add("SdkVersion", "postman-graph/v1.0");
                requestMessage.Content = content;

                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

                var responseContent = await response.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<Token>(responseContent);
                return r.access_token;
            }

            return null;
        }


        //Method to get the Email for the mailbox fuelpurchasing
        public async Task<EmailValue> GetEmail(string tokenString)
        {

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/messages?$filter=subject eq 'Fuel Quote'");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                requestMessage.Headers.Add("SdkVersion", "postman-graph/v1.0");
                // requestMessage.Content = content;

                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();




                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {

                    var emailList = JsonConvert.DeserializeObject<EmailValue>(responseContent);
                    return emailList;


                }
                catch (Exception e)
                {

                    throw;

                }

            }

        }

        //Method
        public async Task<List<FuelArray>> getLovesPrices(string addressURL)
        {
            using (var httpClient = new HttpClient())
            {

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, addressURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {

                    var lovesList = JsonConvert.DeserializeObject<List<FuelArray>>(responseContent);
                    return lovesList;

                }
                catch (Exception e)
                {
                    throw;
                }

            }
        }

        //Actual location where email is sent. Currently no parameters because they are based on the day. Should move to it's own helper folder at some point.
        public void SendEmail()
        {
            List<string> ProcessedNotificationEmails = ConfigurationManager.AppSettings["EmailNotificationList"].Split(';').ToList();
            List<string> ProcessedWinningEmails = ConfigurationManager.AppSettings["EmailWinnerList"].Split(';').ToList();
            string WinningEmailText = ConfigurationManager.AppSettings["EmailWinnerList"].ToString();
            string NotificationEmailText = ConfigurationManager.AppSettings["EmailWinnerList"].ToString();
            try
            {

                Sendmail.SendMail($"Fuel Purchasing Fuel Price for Today", "Today's Fuel Price", NotificationEmailText, "AS1", to: ProcessedNotificationEmails);
                Sendmail.SendMail($"Fuel Purchasing Winner for Today", "Winning Fuel Bid", WinningEmailText, "AS1", to: ProcessedWinningEmails);
            }
            catch (Exception e)
            {

                throw;
            }


        }

        //The following are the classes for the email information from the api call to the fuelpurchasing email. TODO: Move to own file
  

 
    }
}
