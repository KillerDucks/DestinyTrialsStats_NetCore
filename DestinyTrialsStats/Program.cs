using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace DestinyTrialsStats
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            if (CheckForInternetConnection())
            {
                Console.WriteLine("We are Connected to the internet");
                Console.WriteLine("Contacting Bungie Servers ...");
                if (CheckBungieServers())
                {
                    Console.WriteLine("Success!");
                    Console.WriteLine("We are now processing your request");
                    GetTrialsData();
                }
            }
            Console.ReadKey();
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var res = client.GetAsync("https://api.bindserver.com/xAuth/v2/").Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckBungieServers()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var res = client.GetAsync("http://www.bungie.net/").Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetTrialsData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", "1d7e2d7bb0db43618efa3993660282ae");
                    var res = client.GetAsync("http://www.bungie.net/platform/Destiny/Stats/1/4611686018452351232/2305843009326756237/?modes=TrialsOfOsiris").Result;
                    if (res.IsSuccessStatusCode)
                    {
                        string resultContent = res.Content.ReadAsStringAsync().Result;
                        JObject json = JObject.Parse(resultContent);
                        dynamic jsonDe = JsonConvert.DeserializeObject(json.ToString());
                        Console.WriteLine(jsonDe.Response.trialsOfOsiris.allTime.killsDeathsRatio.basic.displayValue);
                        return res.ToString();
                    }
                    else
                    {
                        return "ERROR";
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public static string GetBungieAcc()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var res = client.GetAsync("http://www.bungie.net/").Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return "";
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }
    }
}