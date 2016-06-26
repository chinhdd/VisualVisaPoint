using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Net;
using System.Threading;

namespace Visapoint
{
    public partial class Form1 : Form
    {
        private string country_csvfile_path = "";
        private string firstname = "";
        private string family = "";
        private string birthday = "";
        private string sex = "";
        private string passport = "";
        private string email = "";
        private string phonenumber = "";
        private List<Thread> threadlist = new List<Thread>();
        private List<string> useragentlist = new List<string>();
        private List<string> proxylist = new List<string>();
        private string working_timing = "";
        private string logdirpath = "";
        private string proxydelaytime = "";
        private string timedelay = "";
        public Form1()
        {
            InitializeComponent();
        }
        private double measureTimeOffset(string threadnum,HttpClient httpClient)
        {
            double time_offset = 0;
            echolog(threadnum, "Measure Time Offset");
            int tick1 = Environment.TickCount & Int32.MaxValue;
            var result = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, "https://visapoint.eu/disclaimer")).Result; 
            int responsesecond = result.Headers.Date.Value.Second;
            string post_time = DateTime.Now.ToString("s.fffffff");
            int tick2 = Environment.TickCount & Int32.MaxValue;
            result.Dispose();
            if (Convert.ToDouble(post_time) > responsesecond + (tick2 - tick1) / 2)
            {
                time_offset = responsesecond + ((tick2 - tick1) / 2000) + 60 - Convert.ToDouble(post_time);
            }
            else
                time_offset = responsesecond + ((tick2 - tick1) / 2000) - Convert.ToDouble(post_time);

            if (time_offset < 0)
                time_offset = 60 + time_offset;
            echolog(threadnum, "Time Offset Set To " + time_offset.ToString());
            return time_offset;
        }
        private void echolog(string threadid,string echolog)
        {

            string nowtime = DateTime.Now.ToString("hh:mm:ss.fffffff");
            this.Invoke(new MethodInvoker(
            delegate
            {
                richTextBox1.AppendText("[" + threadid + "]" + "[" + nowtime + "] - " + echolog + "\r\n");
            }
            ));
        }
        private bool FirstPost(string threadnum, HttpClient httpClient, string country, string eventarg, string ddCitizenship, string ddCitizenshipCS, string ddEmbassy, string ddEmbassyCS, string visatype, string visaarg, string visatypenum)
        {
            try
            {
                int proxystarttime = 0; 
                if(proxydelaytime != "all" && proxydelaytime != "")
                {
                    proxystarttime = Convert.ToInt32(DateTime.Now.ToString("mm"));
                }
//                DBC dbc = new DBC("fujiwakama", "Anhyeuem88");
//                string balance = dbc.GetBalance();
                _2Captcha capt = new _2Captcha(textBox17.Text.ToString());
                string balance = capt.GetBalance();

                double time_offset = measureTimeOffset(threadnum.ToString(), httpClient);

                echolog(threadnum, "Bypassing Disclaimer, Load Disclaimer");

                var result = httpClient.GetAsync("https://visapoint.eu/disclaimer").Result;
                string strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                string pattern = "PublicKeyToken(?<VAL>[^\"]*)";
                Match match = Regex.Match(strContent, pattern);
                string rsm1_TSM = match.Groups["VAL"].Value;
                rsm1_TSM = ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken" + rsm1_TSM.Replace("%3a", ":").Replace("%3b", ";").Replace("%3d", "=");

                pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string VIEWSTATE = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string EVENTVALIDATION = match.Groups["VAL"].Value;

                echolog(threadnum, "Bypassing Disclaimer, Post Accept Button");

                result = httpClient.PostAsync("https://visapoint.eu/disclaimer", new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("rsm1_TSM", ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:en-US:93a6b8ed-f453-4cc5-9080-8017894b33b0:ea597d4b:b25378d2;Telerik.Web.UI:en-US:f59d7c8d-045a-4b22-9f9c-e124aae8c608:16e4e7cd:f7645509:22a6274a:ed16cbdc:24ee1bba:2003d0b8:f46195d3:c128760b:1e771326:88144a7a:aa288e2d:b092aa46:6d43f6d9:874f8ea2:19620875:cda80b3"),
                    new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$cp1$btnAccept"),
                    new KeyValuePair<string, string>("__EVENTARGUMENT", ""),
                    new KeyValuePair<string, string>("__VIEWSTATE", VIEWSTATE),
                    new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "3AF36322"),
                    new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""),
                    new KeyValuePair<string, string>("__EVENTVALIDATION", EVENTVALIDATION),
                    new KeyValuePair<string, string>("ctl00_cp1_btnAccept_ClientState", "{\"text\":\"Accept\",\"value\":\"\",\"checked\":false,\"target\":\"\",\"navigateUrl\":\"\",\"commandName\":\"\",\"commandArgument\":\"\",\"autoPostBack\":true,\"selectedToggleStateIndex\":0,\"validationGroup\":null,\"readOnly\":false}"),
                 })).Result;
                result.EnsureSuccessStatusCode();
                result.Dispose();

                echolog(threadnum, "Load Blank Form");

                result = httpClient.GetAsync("https://visapoint.eu/form").Result;
                result.EnsureSuccessStatusCode();
                strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                pattern = "PublicKeyToken(?<VAL>[^\"]*)";
                match = Regex.Match(strContent, pattern);
                rsm1_TSM = match.Groups["VAL"].Value;
                rsm1_TSM = ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken" + rsm1_TSM.Replace("%3a", ":").Replace("%3b", ";").Replace("%3d", "=");

                pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                VIEWSTATE = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                EVENTVALIDATION = match.Groups["VAL"].Value;

                if (EVENTVALIDATION == "" || VIEWSTATE == "")
                {
                    echolog(threadnum, "Can't get info to reselect citizenship");
                    return false;
                }
                var postData1 = new List<KeyValuePair<string, string>>();
                postData1.Add(new KeyValuePair<string, string>("rsm1_TSM", ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:en-US:93a6b8ed-f453-4cc5-9080-8017894b33b0:ea597d4b:b25378d2;Telerik.Web.UI:en-US:f59d7c8d-045a-4b22-9f9c-e124aae8c608:16e4e7cd:f7645509:22a6274a:ed16cbdc:24ee1bba:2003d0b8:f46195d3:c128760b:1e771326:88144a7a:aa288e2d:b092aa46:6d43f6d9"));
                postData1.Add(new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$cp1$ddCitizenship"));
                postData1.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", "{\"Command\":\"Select\",\"Index\":" + eventarg + "}"));
                postData1.Add(new KeyValuePair<string, string>("__VIEWSTATE", VIEWSTATE));
                postData1.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "64675F43"));
                postData1.Add(new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""));
                postData1.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", EVENTVALIDATION));
                postData1.Add(new KeyValuePair<string, string>("ctl00_cp1_ddCitizenship_ClientState", "{\"logEntries\":[],\"value\":\"" + ddCitizenshipCS + "\",\"text\":\"" + ddCitizenship + "\",\"enabled\":true,\"checkedIndices\":[],\"checkedItemsTextOverflows\":false}"));
                postData1.Add(new KeyValuePair<string, string>("ctl00$cp1$ddCitizenship", ddCitizenship));
                postData1.Add(new KeyValuePair<string, string>("ctl00$cp1$ddCountryOfResidence", ddCitizenship));
                postData1.Add(new KeyValuePair<string, string>("ctl00_cp1_ddCountryOfResidence_ClientState", "{\"logEntries\":[],\"value\":\"" + ddCitizenshipCS + "\",\"text\":\"" + ddCitizenship + "\",\"enabled\":true,\"checkedIndices\":[],\"checkedItemsTextOverflows\":false}"));
                postData1.Add(new KeyValuePair<string, string>("ctl00_cp1_btnNext_ClientState", "{\"text\":\"Next\",\"value\":\"\",\"checked\":false,\"target\":\"\",\"navigateUrl\":\"\",\"commandName\":\"next\",\"commandArgument\":\"\",\"autoPostBack\":true,\"selectedToggleStateIndex\":0,\"validationGroup\":null,\"readOnly\":false}"));
                echolog(threadnum, "Reselect citizenship");
                result = httpClient.PostAsync("https://visapoint.eu/form", new MyFormUrlEncodedContent(postData1)).Result;
                result.EnsureSuccessStatusCode();
                strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                pattern = "PublicKeyToken(?<VAL>[^\"]*)";
                match = Regex.Match(strContent, pattern);
                rsm1_TSM = match.Groups["VAL"].Value;
                rsm1_TSM = ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken" + rsm1_TSM.Replace("%3a", ":").Replace("%3b", ";").Replace("%3d", "=");

                pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                VIEWSTATE = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                EVENTVALIDATION = match.Groups["VAL"].Value;

                if (EVENTVALIDATION == "" || VIEWSTATE == "")
                {
                    echolog(threadnum, "Can't get info to prepare checkpoint");
                    return false;
                }
                var postData2 = new List<KeyValuePair<string, string>>();
                postData2.Add(new KeyValuePair<string, string>("rsm1_TSM", ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:en-US:93a6b8ed-f453-4cc5-9080-8017894b33b0:ea597d4b:b25378d2;Telerik.Web.UI:en-US:f59d7c8d-045a-4b22-9f9c-e124aae8c608:16e4e7cd:f7645509:22a6274a:ed16cbdc:24ee1bba:2003d0b8:f46195d3:c128760b:1e771326:88144a7a:aa288e2d:b092aa46:6d43f6d9"));
                postData2.Add(new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$cp1$ddVisaType"));
                postData2.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", "{\"Command\":\"Select\",\"Index\":" + visaarg + "}"));
                postData2.Add(new KeyValuePair<string, string>("__VIEWSTATE", VIEWSTATE));
                postData2.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "64675F43"));
                postData2.Add(new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""));
                postData2.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", EVENTVALIDATION));
                postData2.Add(new KeyValuePair<string, string>("ctl00$cp1$ddVisaType", visatype));
                postData2.Add(new KeyValuePair<string, string>("ctl00_cp1_ddVisaType_ClientState", "{\"logEntries\":[],\"value\":\"" + visatypenum + "\",\"text\":\"" + visatype + "\",\"enabled\":true,\"checkedIndices\":[],\"checkedItemsTextOverflows\":false}"));
                postData2.Add(new KeyValuePair<string, string>("ctl00_cp1_btnNext_ClientState", "{\"text\":\"Next\",\"value\":\"\",\"checked\":false,\"target\":\"\",\"navigateUrl\":\"\",\"commandName\":\"next\",\"commandArgument\":\"\",\"autoPostBack\":true,\"selectedToggleStateIndex\":0,\"validationGroup\":null,\"readOnly\":false}"));
                echolog(threadnum, "Prepare Checkpoint");

                waitUntilValidTime(threadnum,time_offset);

                echolog(threadnum, "Reselect Visa Type");

                result = httpClient.PostAsync("https://visapoint.eu/form", new MyFormUrlEncodedContent(postData2)).Result;
                result.EnsureSuccessStatusCode();
                strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                pattern = "PublicKeyToken(?<VAL>[^\"]*)";
                match = Regex.Match(strContent, pattern);
                rsm1_TSM = match.Groups["VAL"].Value;
                rsm1_TSM = ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken" + rsm1_TSM.Replace("%3a", ":").Replace("%3b", ";").Replace("%3d", "=");

                pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                VIEWSTATE = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                EVENTVALIDATION = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"LBD_VCID_c_pages_form_cp1_captcha1\" id=\"LBD_VCID_c_pages_form_cp1_captcha1\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string lbd = match.Groups["VAL"].Value;

                pattern = "<input id=\"ctl00_cp1_(?<VAL1>([A-Z]{10}))_ClientState\" name=\"ctl00_cp1_(?<VAL2>([A-Z]{10}))_ClientState\" type=\"hidden\" />";
                match = Regex.Match(strContent, pattern);
                string capparam = match.Groups["VAL1"].Value;

                if (EVENTVALIDATION == "" || VIEWSTATE == "")
                {
                    echolog(threadnum, "Can't get info to find slot");
                    return false;
                }

                string capkey = "";
                string jpgurl = "https://visapoint.eu/BotDetectCaptcha.ashx?get=image&amp;c=c_pages_form_cp1_captcha1&amp;t=" + lbd;
                result = httpClient.GetAsync(jpgurl).Result;
                result.EnsureSuccessStatusCode();
                byte[] imagefile = result.Content.ReadAsByteArrayAsync().Result;
                result.Dispose();

                echolog("0", "Your balance is " + balance + " US cents");
                //string cap = dbc.GetCaptchaStr(imagefile);
                //_2Captcha capt = new _2Captcha("78472346b1730205117f1c90fc48a304");
                string cap = capt.GetCaptchaStr(imagefile);

                for (int i = 0; i < cap.Length; i++)
                {
                    string ch = cap.ToUpper().Substring(i, 1);
                    if (i == 0)
                    {
                        capkey = ch;
                        continue;
                    }
                    capkey = capkey + " " + ch;
                }
                echolog(threadnum, "CAPTCHA  solved: " + capkey);
                string KJPLMLSYNX = "{\"enabled\":true,\"emptyMessage\":\"\",\"validationText\":\"" + capkey + "\",\"valueAsString\":\"" + capkey + "\",\"valueWithPromptAndLiterals\":\"" + capkey + "\",\"lastSetTextBoxValue\":\"" + capkey + "\"}";

                var postData3 = new List<KeyValuePair<string, string>>();
                postData3.Add(new KeyValuePair<string, string>("rsm1_TSM", rsm1_TSM));
                postData3.Add(new KeyValuePair<string, string>("__LASTFOCUS", ""));
                postData3.Add(new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$cp1$btnNext"));
                postData3.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
                postData3.Add(new KeyValuePair<string, string>("__VIEWSTATE", VIEWSTATE));
                postData3.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "64675F43"));
                postData3.Add(new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""));
                postData3.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", EVENTVALIDATION));
                postData3.Add(new KeyValuePair<string, string>("ctl00$cp1$" + capparam, capkey));
                postData3.Add(new KeyValuePair<string, string>("ctl00_cp1_" + capparam + "_ClientState", KJPLMLSYNX));
                postData3.Add(new KeyValuePair<string, string>("LBD_VCID_c_pages_form_cp1_captcha1", lbd));
                postData3.Add(new KeyValuePair<string, string>("ctl00_cp1_btnNext_ClientState", "{\"text\":\"Next\",\"value\":\"\",\"checked\":false,\"target\":\"\",\"navigateUrl\":\"\",\"commandName\":\"next\",\"commandArgument\":\"\",\"autoPostBack\":true,\"selectedToggleStateIndex\":0,\"validationGroup\":null,\"readOnly\":false}"));
                waitToPost(threadnum,time_offset);
                echolog(threadnum, "Search For Slot");
                string time_submit = DateTime.Now.ToString("s.fffffff");
                result = httpClient.PostAsync("https://visapoint.eu/form", new MyFormUrlEncodedContent(postData3)).Result;
                result.EnsureSuccessStatusCode();
                strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                pattern = "<input id=\"cp1_rblDate_0\"[^\v]*value=\"(?<VAL>[^\"]*)\" checked=\"checked\" />";
                match = Regex.Match(strContent, pattern);
                string slot = match.Groups["VAL"].Value;

                pattern = "(?<time>[0-9]*:[0-9]*:[0-9]*)";
                match = Regex.Match(strContent, pattern);
                string no_slot_date = match.Groups["time"].Value;

                echolog(threadnum, "Process Result");

                if (slot != "")
                {
                    echolog(threadnum, "Slot available!");
                    return postFinalStep(slot,strContent,threadnum,httpClient, country,  eventarg,  ddCitizenship,  ddCitizenshipCS,  ddEmbassy,  ddEmbassyCS,  visatype,  visaarg,  visatypenum);                    
                }
                double time_process = 0;
                if (no_slot_date != "")
                    echolog(threadnum, "No slot - Time return " + no_slot_date);
                else
                    echolog(threadnum, "Don't pass captcha " + no_slot_date);
                if (Convert.ToDouble(DateTime.Now.ToString("s.fffffff")) < Convert.ToDouble(time_submit))
                    time_process = Convert.ToDouble(DateTime.Now.ToString("s.fffffff")) + 60 - Convert.ToDouble(time_submit);
                else
                    time_process = Convert.ToDouble(DateTime.Now.ToString("s.fffffff")) - Convert.ToDouble(time_submit);
                echolog("0", "Process Time: " + time_process);
                if (no_slot_date != "")
                {
                    if (no_slot_date.Split(':')[2] == "59")
                        time_offset = time_offset - (Convert.ToDouble(textBox13.Text) * 3);
                    if (no_slot_date.Split(':')[2] == "00")
                        time_offset = time_offset + Convert.ToDouble(textBox13.Text);
                }
                string returnvalue = SecondPost(proxystarttime,postData2,time_offset,strContent,threadnum, httpClient, country, eventarg, ddCitizenship, ddCitizenshipCS, ddEmbassy, ddEmbassyCS, visatype, visaarg, visatypenum);
                if (returnvalue == "0")
                {
                    return false;
                }
                if (returnvalue == "1")
                {
                    return true;
                }

                pattern = "(?<time>[0-9]*:[0-9]*:[0-9]*)";
                match = Regex.Match(returnvalue, pattern);
                no_slot_date = match.Groups["time"].Value;
                if (no_slot_date != "")
                {
                    if (no_slot_date.Split(':')[2] == "59")
                        time_offset = time_offset - (Convert.ToDouble(textBox13.Text) * 3);
                    if (no_slot_date.Split(':')[2] == "00")
                        time_offset = time_offset + Convert.ToDouble(textBox13.Text);
                    if (no_slot_date.Split(':')[2] == "58")
                        time_offset = time_offset - (Convert.ToDouble(textBox13.Text) * 10);
                    if (no_slot_date.Split(':')[2] == "01")
                        time_offset = time_offset + (Convert.ToDouble(textBox13.Text) * 10);
                }

                while(true)
                {
                    returnvalue = SecondPost(proxystarttime,postData2,time_offset, returnvalue, threadnum, httpClient, country, eventarg, ddCitizenship, ddCitizenshipCS, ddEmbassy, ddEmbassyCS, visatype, visaarg, visatypenum);
                    if(returnvalue == "0")
                    {
                        return false;
                    }
                    if(returnvalue == "1")
                    {
                        return true;
                    }
                    pattern = "(?<time>[0-9]*:[0-9]*:[0-9]*)";
                    match = Regex.Match(returnvalue, pattern);
                    no_slot_date = match.Groups["time"].Value;
                    if (no_slot_date != "")
                    {
                        if (no_slot_date.Split(':')[2] == "59")
                            time_offset = time_offset - (Convert.ToDouble(textBox13.Text) * 3);
                        if (no_slot_date.Split(':')[2] == "00")
                            time_offset = time_offset + Convert.ToDouble(textBox13.Text);
                        if (no_slot_date.Split(':')[2] == "58")
                            time_offset = time_offset - (Convert.ToDouble(textBox13.Text) * 10);
                        if (no_slot_date.Split(':')[2] == "01")
                            time_offset = time_offset + (Convert.ToDouble(textBox13.Text) * 10);
                    }
                    Thread.Sleep(100);               
                }
            }
            catch(Exception ex)
            {
                echolog(threadnum, ex.ToString());
                return false;
            }
        }

        private string SecondPost(int proxytime,List<KeyValuePair<string, string>> postData2,double time_offset,string returnhtml,string threadnum, HttpClient httpClient, string country, string eventarg, string ddCitizenship, string ddCitizenshipCS, string ddEmbassy, string ddEmbassyCS, string visatype, string visaarg, string visatypenum)
        {
            try
            {
                if(proxytime != 0)
                {
                    int cometime = 0;
                    int nowmin = Convert.ToInt32(DateTime.Now.ToString("mm"));
                    if (nowmin < proxytime)
                    {
                        cometime = 60 - proxytime + nowmin;
                    }
                    else
                        cometime = nowmin - proxytime;
                    if (cometime >= Convert.ToInt32(proxydelaytime))
                        return "0";
                }

//                DBC dbc = new DBC("fujiwakama", "Anhyeuem88");
//                string balance = dbc.GetBalance();                
                _2Captcha capt = new _2Captcha(textBox17.Text.ToString());
                string balance = capt.GetBalance();

                echolog(threadnum, "New time offset is: " + time_offset);

                waitUntilValidTime(threadnum, time_offset);

                echolog(threadnum, "Reselect Visa Type");

                var result = httpClient.PostAsync("https://visapoint.eu/form", new MyFormUrlEncodedContent(postData2)).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                string pattern = "PublicKeyToken(?<VAL>[^\"]*)";
                Match match = Regex.Match(strContent, pattern);
                string rsm1_TSM = match.Groups["VAL"].Value;
                rsm1_TSM = ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken" + rsm1_TSM.Replace("%3a", ":").Replace("%3b", ";").Replace("%3d", "=");

                pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string VIEWSTATE = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string EVENTVALIDATION = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"LBD_VCID_c_pages_form_cp1_captcha1\" id=\"LBD_VCID_c_pages_form_cp1_captcha1\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string lbd = match.Groups["VAL"].Value;

                pattern = "<input id=\"ctl00_cp1_(?<VAL1>([A-Z]{10}))_ClientState\" name=\"ctl00_cp1_(?<VAL2>([A-Z]{10}))_ClientState\" type=\"hidden\" />";
                match = Regex.Match(strContent, pattern);
                string capparam = match.Groups["VAL1"].Value;

                if (EVENTVALIDATION == "" || VIEWSTATE == "")
                {
                    echolog(threadnum, "Can't get info to find slot");
                    return "0";
                }
                string capkey = "";
                string jpgurl = "https://visapoint.eu/BotDetectCaptcha.ashx?get=image&amp;c=c_pages_form_cp1_captcha1&amp;t=" + lbd;
                result = httpClient.GetAsync(jpgurl).Result;
                result.EnsureSuccessStatusCode();
                byte[] imagefile = result.Content.ReadAsByteArrayAsync().Result;
                result.Dispose();

                echolog("0", "Your balance is " + balance + " US cents");
                //string cap = dbc.GetCaptchaStr(imagefile);
                string cap = capt.GetCaptchaStr(imagefile);
                for (int i = 0; i < cap.Length; i++)
                {
                    string ch = cap.ToUpper().Substring(i, 1);
                    if (i == 0)
                    {
                        capkey = ch;
                        continue;
                    }
                    capkey = capkey + " " + ch;
                }
                echolog(threadnum, "CAPTCHA  solved: " + capkey);
                string KJPLMLSYNX = "{\"enabled\":true,\"emptyMessage\":\"\",\"validationText\":\"" + capkey + "\",\"valueAsString\":\"" + capkey + "\",\"valueWithPromptAndLiterals\":\"" + capkey + "\",\"lastSetTextBoxValue\":\"" + capkey + "\"}";

                var postData3 = new List<KeyValuePair<string, string>>();
                postData3.Add(new KeyValuePair<string, string>("rsm1_TSM", ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35:en-US:93a6b8ed-f453-4cc5-9080-8017894b33b0:ea597d4b:b25378d2;Telerik.Web.UI:en-US:f59d7c8d-045a-4b22-9f9c-e124aae8c608:16e4e7cd:f7645509:22a6274a:ed16cbdc:24ee1bba:2003d0b8:f46195d3:c128760b:1e771326:88144a7a:aa288e2d:b092aa46:6d43f6d9"));
                postData3.Add(new KeyValuePair<string, string>("__LASTFOCUS", ""));
                postData3.Add(new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$cp1$btnNext"));
                postData3.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
                postData3.Add(new KeyValuePair<string, string>("__VIEWSTATE", VIEWSTATE));
                postData3.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "64675F43"));
                postData3.Add(new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""));
                postData3.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", EVENTVALIDATION));
                postData3.Add(new KeyValuePair<string, string>("ctl00$cp1$" + capparam, capkey));
                postData3.Add(new KeyValuePair<string, string>("ctl00_cp1_" + capparam + "_ClientState", KJPLMLSYNX));
                postData3.Add(new KeyValuePair<string, string>("LBD_VCID_c_pages_form_cp1_captcha1", lbd));
                postData3.Add(new KeyValuePair<string, string>("ctl00_cp1_btnNext_ClientState", "{\"text\":\"Next\",\"value\":\"\",\"checked\":false,\"target\":\"\",\"navigateUrl\":\"\",\"commandName\":\"next\",\"commandArgument\":\"\",\"autoPostBack\":true,\"selectedToggleStateIndex\":0,\"validationGroup\":null,\"readOnly\":false}"));
                waitToPost(threadnum, time_offset);
                echolog(threadnum, "Search For Slot");
                string time_submit = DateTime.Now.ToString("s.fffffff");
                result = httpClient.PostAsync("https://visapoint.eu/form", new MyFormUrlEncodedContent(postData3)).Result;
                result.EnsureSuccessStatusCode();
                strContent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                pattern = "<input id=\"cp1_rblDate_0\"[^\v]*value=\"(?<VAL>[^\"]*)\" checked=\"checked\" />";
                match = Regex.Match(strContent, pattern);
                string slot = match.Groups["VAL"].Value;

                pattern = "(?<time>[0-9]*:[0-9]*:[0-9]*)";
                match = Regex.Match(strContent, pattern);
                string no_slot_date = match.Groups["time"].Value;

                echolog(threadnum, "Process Result");

                if (slot != "")
                {
                    echolog(threadnum, "Slot available!");
                    bool resbool = postFinalStep(slot, strContent, threadnum, httpClient, country, eventarg, ddCitizenship, ddCitizenshipCS, ddEmbassy, ddEmbassyCS, visatype, visaarg, visatypenum);
                    if (resbool) return "1";
                }
                double time_process = 0;
                if (no_slot_date != "")
                    echolog(threadnum, "No slot - Time return " + no_slot_date);
                else
                    echolog(threadnum, "Don't pass captcha " + no_slot_date);
                if (Convert.ToDouble(DateTime.Now.ToString("s.fffffff")) < Convert.ToDouble(time_submit))
                    time_process = Convert.ToDouble(DateTime.Now.ToString("s.fffffff")) + 60 - Convert.ToDouble(time_submit);
                else
                    time_process = Convert.ToDouble(DateTime.Now.ToString("s.fffffff")) - Convert.ToDouble(time_submit);
                echolog("0", "Process Time: " + time_process);
                return strContent;
            }
            catch(Exception ex)
            {
                echolog(threadnum, ex.ToString());
                return "0";
            }
        }
        private void waitUntilValidTime(string threadnum, double time_offset)
        {
            echolog(threadnum, "Wait Until Valid Time");
            while(true)
            {
                string now = DateTime.Now.ToString("s.fffffff");
                double current = Convert.ToDouble(now) + time_offset;
                if (current >= 60)
                    current = current - 60;
                if (current >= Convert.ToDouble(textBox10.Text) - 19 && current <= Convert.ToDouble(textBox10.Text) - 15)
                    return;
                Thread.Sleep(1);
            }
        }

        private void waitToPost(string threadnum,double time_offset)
        {
            echolog(threadnum, "Wait To Post");
            while(true)
            {
                string now = DateTime.Now.ToString("s.fffffff");
                double current = Convert.ToDouble(now) + time_offset;
                if (current >= 60)
                    current = current - 60;
                if (current >= Convert.ToDouble(textBox10.Text))
                    break;
                Thread.Sleep(1);
            }
        }
        private bool postFinalStep(string slot,string strContent, string threadnum, HttpClient httpClient, string country, string eventarg, string ddCitizenship, string ddCitizenshipCS, string ddEmbassy, string ddEmbassyCS, string visatype, string visaarg, string visatypenum)
        {
            try
            {
                echolog(threadnum,"Post Final Step");
                /////////////////////////////adddddd param/////////////////////////////////////
                string txtfirstname = "{\"enabled\":true,\"emptyMessage\":\"Fill in your first name(s)\",\"validationText\":\"" + firstname + "\",\"valueAsString\":\"" + firstname + "\",\"lastSetTextBoxValue\":\"" + firstname + "\"}";
                string txtfamily = "{\"enabled\":true,\"emptyMessage\":\"Fill in your Family Name(s)\",\"validationText\":\"" + family + "\",\"valueAsString\":\"" + family + "\",\"lastSetTextBoxValue\":\"" + family + "\"}";
                string[] birspilt = birthday.Split('-');
                if (birspilt[1].Substring(0, 1) == "0") birspilt[1] = birspilt[1].Substring(1, 1);
                if (birspilt[2].Substring(0, 1) == "0") birspilt[2] = birspilt[2].Substring(1, 1);
                string birthinput = birspilt[1] + "/" + birspilt[2] + "/" + birspilt[0];
                string nowdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                string[] nowdatespil = nowdate.Split('-');
                if (nowdatespil[1].Substring(0, 1) == "0") nowdatespil[1] = nowdatespil[1].Substring(1, 1);
                if (nowdatespil[2].Substring(0, 1) == "0") nowdatespil[2] = nowdatespil[2].Substring(1, 1);
                string dateinput = "{\"enabled\":true,\"emptyMessage\":\"\",\"validationText\":\"" + birthday + "-00-00-00" + "\",\"valueAsString\":\"" + birthday + "-00-00-00" + "\",\"minDateStr\":\"1896-03-31-00-00-00\",\"maxDateStr\":\"" + nowdate + "-00-00-00" + "\",\"lastSetTextBoxValue\":\"" + birthinput + "\"}";
                string calendaAD = "[[1896,3,31],[" + nowdatespil[0] + "," + nowdatespil[1] + "," + nowdatespil[2] + "],[" + birspilt[0] + "," + birspilt[1] + "," + birspilt[2] + "]]";
                string calendaSD = "[[" + birspilt[0] + "," + birspilt[1] + "," + birspilt[2] + "]]";
                string txtpassport = "{\"enabled\":true,\"emptyMessage\":\"Fill in your passport number\",\"validationText\":\"" + passport + "\",\"valueAsString\":\"" + passport + "\",\"lastSetTextBoxValue\":\"" + passport + "\"}";
                string txtemail = "{\"enabled\":true,\"emptyMessage\":\"Fill in valid email\",\"validationText\":\"" + email + "\",\"valueAsString\":\"" + email + "\",\"lastSetTextBoxValue\":\"" + email + "\"}";
                string txtphonenumber = "{\"enabled\":true,\"emptyMessage\":\"Fill in your phone number\",\"validationText\":\"" + phonenumber.Replace("_", "") + "\",\"valueAsString\":\"" + phonenumber + "\",\"valueWithPromptAndLiterals\":\"" + phonenumber + "\",\"lastSetTextBoxValue\":\"" + phonenumber + "\"}";
                string sexD = "";
                if (sex == "M") sexD = "Male";
                else sexD = "Female";
                string txtsex = "{\"logEntries\":[],\"value\":\"" + sex + "\",\"text\":\"" + sexD + "\",\"enabled\":true,\"checkedIndices\":[],\"checkedItemsTextOverflows\":false}";
                string txtcountry = "{\"logEntries\":[],\"value\":\"12\",\"text\":\"" + ddCitizenship + "\",\"enabled\":true,\"checkedIndices\":[],\"checkedItemsTextOverflows\":false}";

                string pattern = "PublicKeyToken(?<VAL>[^\"]*)";
                Match match = Regex.Match(strContent, pattern);
                string rsm1_TSM = match.Groups["VAL"].Value;

                rsm1_TSM = ";;System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken" + rsm1_TSM.Replace("%3a", ":").Replace("%3b", ";").Replace("%3d", "=");

                pattern = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string VIEWSTATE = match.Groups["VAL"].Value;

                pattern = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<VAL>[^\"]*)\" />";
                match = Regex.Match(strContent, pattern);
                string EVENTVALIDATION = match.Groups["VAL"].Value;
                if (EVENTVALIDATION == "" || VIEWSTATE == "")
                {
                    echolog(threadnum, "Can't get info to post final step. Probably blocked.");
                    return false;
                }
                ///////////////////////////////////////////////////////////////////////////////
                var fillpostData = new List<KeyValuePair<string, string>>();
                fillpostData.Add(new KeyValuePair<string, string>("rsm1_TSM", rsm1_TSM));
                fillpostData.Add(new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$cp1$btnSend"));
                fillpostData.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
                fillpostData.Add(new KeyValuePair<string, string>("__VIEWSTATE", VIEWSTATE));
                fillpostData.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "64675F43"));
                fillpostData.Add(new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""));
                fillpostData.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", EVENTVALIDATION));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_btnSend_ClientState", ""));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$rblDate", slot));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtFirstName", firstname));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtFirstName_ClientState", txtfirstname));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtFamilyName", family));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtFamilyName_ClientState", txtfamily));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtBirthDate", birthday));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtBirthDate$dateInput", birthinput));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtBirthDate_dateInput_ClientState", dateinput));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtBirthDate_calendar_SD", calendaSD));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtBirthDate_calendar_AD", calendaAD));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtBirthDate_ClientState", "{\"minDateStr\":\"1896-03-31-00-00-00\",\"maxDateStr\":\"" + nowdate + "-00-00-00" + "\"}"));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$ddBirthCountry", ddCitizenship));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_ddBirthCountry_ClientState", txtcountry));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$ddSex", sexD));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_ddSex_ClientState", txtsex));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtPassportNumber", passport));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtPassportNumber_ClientState", txtpassport));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtEmail", email));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtEmail_ClientState", txtemail));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00$cp1$txtPhone", phonenumber));
                fillpostData.Add(new KeyValuePair<string, string>("ctl00_cp1_txtPhone_ClientState", txtphonenumber));
                httpClient.DefaultRequestHeaders.Referrer = new Uri("https://visapoint.eu/form");
                var result = httpClient.PostAsync("https://visapoint.eu/form", new MyFormUrlEncodedContent(fillpostData)).Result;

                result.EnsureSuccessStatusCode();
                string strcontent = result.Content.ReadAsStringAsync().Result;
                result.Dispose();

                if (strcontent.Contains("Please, cancel it first.") == true)
                {
                    echolog(threadnum, "Submit Post is error. Exist error.\"You have an active appointment in database. Please, cancel it first.\"\r\n");
                    return false;
                }
                if (strContent.Contains("Appointment Tracking Number") != true)
                {
                    echolog(threadnum, "Personal info is error\r\n");
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                echolog("0", ex.ToString());
                return false;
            }
        }
        private void ProcessThread(int threadnum,string country, string eventarg, string ddCitizenship, string ddCitizenshipCS, string ddEmbassy, string ddEmbassyCS, string visatype, string visaarg, string visatypenum)
        {
            string starttime = "";
            string endtime = "";
            if(working_timing != "All")
            {
                string[] se = working_timing.Split('-');
                starttime = se[0];
                endtime = se[1];
                while(true)
                {
                    string now = DateTime.Now.ToString("HHmm");
                    if (Convert.ToInt32(starttime) >= Convert.ToInt32(now))
                        break;
                    Thread.Sleep(60000);
                }
            }
            int i = 0;
            while(true)
            {
                Random r = new Random();
                System.Net.Http.HttpClient httpClient = null;
                if (proxydelaytime != "")
                {
                    int count = i % proxylist.Count;
                    var httpClientHandler = new HttpClientHandler
                    {
                        Proxy = new WebProxy(proxylist[count], false),
                        UseProxy = true
                    };
                    httpClient = new HttpClient(httpClientHandler);
                    echolog(threadnum.ToString(), "Current Proxy => " + proxylist[count]);
                }
                else
                    httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", useragentlist[r.Next(0, useragentlist.Count - 1)]);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "visapoint.eu");
                if (FirstPost(threadnum.ToString(), httpClient, country, eventarg, ddCitizenship, ddCitizenshipCS, ddEmbassy, ddEmbassyCS, visatype, visaarg, visatypenum))
                {                
                    echolog(threadnum.ToString(), threadnum.ToString() + " thread exist successfully!");
                    threadlist.RemoveAt(threadnum);
                    return;
                 }
                else
                {
                    if (proxydelaytime == "")
                    {
                        echolog(threadnum.ToString(), "Your ip Address blocked. Thread is Delaying.");
                        Thread.Sleep(Convert.ToInt32(timedelay) * 60 * 1000);
                        continue;
                    }
                }
                i++;
                if(endtime != "")
                {
                    string now = DateTime.Now.ToString("HHmm");
                    if (Convert.ToInt32(endtime) <= Convert.ToInt32(now))
                        return;
                }
            }   
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button5.Enabled = true;
            if(textBox10.Text == "")
            {
                MessageBox.Show("Please input post time!");
                button1.Enabled = true;
                button5.Enabled = false;
                return;
            }
            if(useragentlist.Count == 0)
            {
                MessageBox.Show("Please input Agent list txt file!");
                button1.Enabled = true;
                button5.Enabled = false;
                return;
            }
            if (textBox8.Text == "")
            {
                MessageBox.Show("Please input csv file!");
                button1.Enabled = true;
                button5.Enabled = false;
                return;
            }
            
            if(textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
            {
                MessageBox.Show("Profile information input!");
                button1.Enabled = true;
                button5.Enabled = false;
                return;
            }
            if(listView2.Items.Count == 0)
            {
                MessageBox.Show("No selected country!");
                button1.Enabled = true;
                button5.Enabled = false;
                return;
            }
            if(radioButton1.Checked)
            {
                if(proxylist.Count == 0) 
                {
                    MessageBox.Show("No existing Proxylist!");
                    button1.Enabled = true;
                    button5.Enabled = false;
                    return;
                }
                if (comboBox2.SelectedIndex == 0)
                {
                    proxydelaytime = "all";                    
                }                    
                else
                {
                    proxydelaytime = textBox16.Text.ToString();
                }
                timedelay = "0";
            }
            else
            {
                proxydelaytime = "";
                timedelay = textBox15.Text;
            }
            if (radioButton4.Checked)
            {
                if (textBox14.Text == "")
                {
                    MessageBox.Show("No existing working time!");
                    button1.Enabled = true;
                    button5.Enabled = false;
                    return;
                }
                working_timing = textBox14.Text.ToString();
            }
            else
                working_timing = "All";
            country_csvfile_path = textBox8.Text.ToString();
            firstname = textBox2.Text.ToString();
            family = textBox3.Text.ToString();
            birthday = textBox4.Text.ToString();
            if (comboBox1.SelectedItem.ToString() == "Male")
                sex = "M";
            else
                sex = "F";
            passport = textBox5.Text.ToString();
            email = textBox6.Text.ToString();
            phonenumber = textBox7.Text.ToString();
            richTextBox1.AppendText("Program start......\r\n");
            int i = 0;
            StreamReader sr = new StreamReader(country_csvfile_path,false);
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                string[] temp = s.Split(',');
                foreach(ListViewItem view in listView2.Items)
                {
                    if (view.Text.Contains(temp[0]) && view.Text.Contains(temp[6]))
                    {
                        Thread thread = new Thread(delegate()
                        {
                            ProcessThread(i,temp[0], temp[1], temp[2], temp[3], temp[4], temp[5], temp[6], temp[7], temp[8]);
                        });
                        thread.Start();
                        Thread.Sleep(1000);
                        threadlist.Add(thread);
                        i++;
                    }
                }                
            }            
            if(threadlist.Count == 0)
            {
                button1.Enabled = true;
                button5.Enabled = false;
                return;
            }
            textBox11.Text = threadlist.Count.ToString();
            new Thread(GetThreadStatus).Start();
        }
        
        private void GetThreadStatus()
        {
            int i = 0;
            while(true)
            {
                string starttime = "";
                string endtime = "";
                if (working_timing != "All")
                {
                    string[] se = working_timing.Split('-');
                    starttime = se[0];
                    endtime = se[1];
                    while (true)
                    {
                        string now = DateTime.Now.ToString("HHMM");
                        if (Convert.ToInt32(starttime) >= Convert.ToInt32(now))
                            break;
                        Thread.Sleep(60000);
                    }
                }
                i = 0;
                foreach(Thread thread in threadlist)
                {
                    if (thread.IsAlive)
                        i++;
                    else
                        threadlist.RemoveAt(i);
                }
                if (i == 0)
                {
                    this.Invoke(new MethodInvoker(
                    delegate
                    {
                        textBox11.Text = i.ToString();
                        button1.Enabled = true;
                        button5.Enabled = false;
                    }
                    ));                    
                    return;
                }
                else
                {
                    this.Invoke(new MethodInvoker(
                    delegate
                    {
                        textBox11.Text = i.ToString();
                    }
                    ));
                }
                Thread.Sleep(1000);
                if (endtime == "")
                    continue;
                string nowtime = DateTime.Now.ToString("HHMM");
                if (Convert.ToInt32(nowtime) >= Convert.ToInt32(endtime))
                {
                    threadlist.Clear();
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            textBox1.Visible = false;
            textBox4.Text = "1996-03-01";
            textBox4.ForeColor = Color.Gray;
            textBox7.Text = "+(87) 912345679";
            textBox7.ForeColor = Color.Gray;
            string sDirPath;
            sDirPath = Application.StartupPath + "\\log";
            DirectoryInfo di = new DirectoryInfo(sDirPath);
            if (di.Exists == false)
            {
                di.Create();
            }
            logdirpath = sDirPath;
            comboBox2.SelectedIndex = 0;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button9.Visible = true;            
            comboBox2.Visible = true;
            textBox15.Enabled = false;
            if(comboBox2.SelectedIndex == 1)
                textBox16.Visible = true;
            else
                textBox16.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button9.Visible = false;
            comboBox2.Visible = false;
            textBox15.Enabled = true;
            textBox16.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string original = "";
                textBox8.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(textBox8.Text.ToString(), false);
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string temp = s.Split(',')[0] + "-" + s.Split(',')[6];
                    //if (original == temp) continue;
                    listView1.Items.Add(temp);
                    original = temp;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem view in listView1.SelectedItems)
            {
                listView2.Items.Add(view.Text);
                listView1.Items.Remove(view);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem view in listView2.SelectedItems)
            {
                listView1.Items.Add(view.Text);
                listView2.Items.Remove(view);
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox4.ForeColor = Color.Black;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if(textBox4.Text.Trim().Length==0)
            {
                textBox4.ForeColor = Color.LightGray;
                textBox4.Text = "1996-03-01";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox9.Text = System.DateTime.Now.ToString("hh:mm:ss.fff");
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            textBox7.Text = "";
            textBox7.ForeColor = Color.Black;
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (textBox7.Text.Trim().Length == 0)
            {
                textBox7.ForeColor = Color.LightGray;
                textBox7.Text = "+(87) 912345679";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach(Thread thread in threadlist)
            {
                thread.Abort();
            }
            button1.Enabled = true;
            button5.Enabled = false;
            threadlist.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "text files (*.txt)|*.txt";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox12.Text = openFileDialog1.FileName;
                string[] agentlist = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                foreach(string str in agentlist)
                {
                    useragentlist.Add(str);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (Thread thread in threadlist)
            {
                thread.Abort();
            }
            Thread.Sleep(200);
            Application.Exit();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox14.Visible = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox14.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String txt = richTextBox1.Text;
            File.WriteAllText(logdirpath + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt", txt);
            MessageBox.Show(logdirpath + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt" + "  is saved");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "text files (*.txt)|*.txt";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                string[] agentlist = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                foreach (string str in agentlist)
                {
                    proxylist.Add(str);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Run until blocked")
            {
                textBox16.Visible = false;
            }
            else
            {
                textBox16.Visible = true;                
            }
        }
    }
}
