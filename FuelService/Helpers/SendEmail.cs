using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FuelService.Helpers
{
    public enum EmailImportance
    {
        High,
        Normal,
        Low
    }
    public sealed class Sendmail
    {
        public static void SendMail(string subject, string body, string emailType, string sentBy, Dictionary<string, byte[]> attachments = null, EmailImportance importance = EmailImportance.Normal, List<string> to = null, List<string> cc = null, List<string> bcc = null, int? dataKey = null, string dataKeyColumn = "None")
        {
            EmailPayLoad emailPayLoad = new EmailPayLoad
            {
                application = "Fuel Purchasing",
                emailType = "FuelPurchasing",
                dataKey = 1,
                dataKeyColumn = "OrderId",
                from = "noreply@carter-logistics.com",
                to = to,
                cc = cc,
                bcc = bcc,
                //  emailIt = false,

                subject = subject,



                body = body,

                attachments = attachments,
                sentBy = sentBy
            };
            var url = ConfigurationManager.AppSettings["EmailMicroService"] + "http://emailms/api/EmailService/sendMail";
            var payload = JsonConvert.SerializeObject(emailPayLoad);

            HttpResponseMessage response = null;

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(
                    payload,
                    Encoding.UTF8,
                    "application/json")
            };
            using (var client =
                   new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            {


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                response = client.SendAsync(request).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var stringdata = response.Content.ReadAsStringAsync();
                    throw new Exception(stringdata.Result);
                }
            }


        }
    }

    public class EmailPayLoad
    {
        public string application { get; set; }

        public string emailType { get; set; }

        public int dataKey { get; set; }

        public string dataKeyColumn { get; set; }

        public string from { get; set; }

        public string password { get; set; }

        public List<string> to { get; set; }

        public List<string> cc { get; set; }

        public List<string> bcc { get; set; }

        public Dictionary<string, byte[]> attachments { get; set; }

        public string subject { get; set; }

        public string body { get; set; }

        public string sentBy { get; set; }

        public bool emailIt { get; set; }

    }
}
