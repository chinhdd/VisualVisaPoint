using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecaptcherLib;
using System.Net.Http;

namespace Visapoint
{
    class DBC
    {
        private string username = "";
        private string password = "";
        private string major = "";
        private string min = "";
        private HttpClient httpClient = new HttpClient();
        public DBC(string deusrname,string depassword)
        {
            username = deusrname;
            password = depassword;
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36");
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "de-captcher.com");
        }
        public string GetBalance()
        {
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                var values = new[]
                {
                    new KeyValuePair<string, string>("function", "balance"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("submit", "Send"),
                };
                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value),
                        String.Format("\"{0}\"", keyValuePair.Key));
                }
                var result = httpClient.PostAsync("http://poster.de-captcher.com/", content).Result;
                return result.Content.ReadAsStringAsync().Result;                
            }
            catch
            {
                return "";
            }
        }
        public string GetCaptchaStr(byte[] imagefile)
        {
            try
            {
                //var result = httpClient.GetAsync("http://poster.de-captcher.com/").Result;
                //result.Dispose();
                
                MultipartFormDataContent content = new MultipartFormDataContent();
                var values = new[]
                {
                    new KeyValuePair<string, string>("function", "picture2"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("pict_type", "0"),
                    new KeyValuePair<string, string>("submit", "Send"),
                };
                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value),
                        String.Format("\"{0}\"", keyValuePair.Key));
                }
                content.Add(new StreamContent(new MemoryStream(imagefile)), "pict");
                var result = httpClient.PostAsync("http://poster.de-captcher.com/", content).Result;
                string strContent = result.Content.ReadAsStringAsync().Result;
                major = strContent.Split('|')[1];
                min = strContent.Split('|')[2];
                return strContent.Split('|')[5];
            }
            catch
            {
                return "";
            }            
        }
        public bool InsistencebadCharacter()
        {
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                var values = new[]
                {
                    new KeyValuePair<string, string>("function", "picture_bad2"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("major_id", major),
                    new KeyValuePair<string, string>("minor_id", min),
                    new KeyValuePair<string, string>("submit", "Send"),
                };
                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value),
                        String.Format("\"{0}\"", keyValuePair.Key));
                }
                var result = httpClient.PostAsync("http://poster.de-captcher.com/", content).Result;
                string strContent = result.Content.ReadAsStringAsync().Result;                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
