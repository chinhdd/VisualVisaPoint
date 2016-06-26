using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace Visapoint
{
    class _2Captcha
    {
        private string key = "";
        private HttpClient httpClient = new HttpClient();
        public _2Captcha(string keyvalue)
        {
            key = keyvalue;
        }
        public string GetBalance()
        {
            try
            {
                var result = httpClient.GetAsync("http://2captcha.com/res.php?key=" + key + "&action=getbalance").Result;
                string strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();
                return strContent; 
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
                string str = Convert.ToBase64String(imagefile);
                MultipartFormDataContent content = new MultipartFormDataContent();
                var values = new[]
                {
                    new KeyValuePair<string, string>("key", key),
                    new KeyValuePair<string, string>("method", "base64"),
                    new KeyValuePair<string, string>("body", str),
                    new KeyValuePair<string, string>("submit", "download and get the ID"),
                };
                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value),
                        String.Format("\"{0}\"", keyValuePair.Key));
                }
                var result = httpClient.PostAsync("http://2captcha.com/in.php", content).Result;
                string strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();
                Thread.Sleep(2000);
                if(strContent.Contains("OK"))
                {
                    string id = strContent.Split('|')[1];
                    for(int i = 0; i < 100 ; i++)
                    {
                        result = httpClient.GetAsync("http://2captcha.com/res.php?key=" + key + "&action=get&id=" + id).Result;
                        strContent = result.Content.ReadAsStringAsync().Result;
                        result.Dispose();
                        if(strContent.Contains("OK"))
                        {
                            return strContent.Split('|')[1];
                        }
                        Thread.Sleep(200);
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
