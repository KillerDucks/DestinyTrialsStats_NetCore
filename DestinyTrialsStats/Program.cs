using System;
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
                    //GetTrialsData();
                    if (1 == 1/*args[1] == "GetBungieAcc"*/)
                    {
                        if (GetBungieAcc("TacticalNexus4") == "SUCCESS")
                        {
                            Console.WriteLine("We are at if: 1");
                            var xy = GetDestinyAccount();
                            if (xy == "SUCCESS")
                            {
                                Console.WriteLine("We are at if: 2");
                                GetTrialsData();
                            } else
                            {
                                Console.WriteLine("Error on 2nd If");
                                Console.WriteLine("Bungie Member ID: " + PlayerCharater.BungMembID);
                                Console.WriteLine(xy);
                            }
                        } else
                        {
                            Console.WriteLine("Error on 1st If");
                        }
                    }
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
                    for (int i = 0; i < 3; i++)
                    {
                        var res = client.GetAsync("https://www.bungie.net/platform/Destiny/Stats/1/" + PlayerCharater.DestMembID + "/" + PlayerCharater.Characters[i] + "/?modes=TrialsOfOsiris").Result;
                        if (res.IsSuccessStatusCode)
                        {
                            string resultContent = res.Content.ReadAsStringAsync().Result;
                            JObject json = JObject.Parse(resultContent);
                            dynamic jsonDe = JsonConvert.DeserializeObject(json.ToString());
                            Console.WriteLine(PlayerCharater.CharactersInfo[i]);
                            Console.WriteLine(jsonDe.Response.trialsOfOsiris.allTime.killsDeathsRatio.basic.displayValue);
                        }
                        else
                        {
                            Console.WriteLine("TrialsData Error");
                            return "ERROR";
                        }
                    }
                    return "PRINTED TO SCREEN";
                }
            }
            catch
            {
                return "";
            }
        }

        public static string GetBungieAcc(string GamerTag)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", "1d7e2d7bb0db43618efa3993660282ae");
                    var res = client.GetAsync("https://www.bungie.net/Platform/User/SearchUsers/?q=" + GamerTag).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        string resultContent = res.Content.ReadAsStringAsync().Result;
                        JObject json = JObject.Parse(resultContent);
                        dynamic jsonDe = JsonConvert.DeserializeObject(json.ToString());
                        foreach(var i in jsonDe)
                        {
                            if (jsonDe.Response[0].xboxDisplayName == GamerTag)
                            {
                                PlayerCharater.BungMembID = jsonDe.Response[0].membershipId;
                                Console.WriteLine(jsonDe.Response[0].membershipId);
                                return "SUCCESS";
                            }
                            else
                            {
                                return "NOID";
                            }
                        }

                        return "Error in ForEach In";
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

        public static string GetDestinyAccount()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", "1d7e2d7bb0db43618efa3993660282ae");
                    var res = client.GetAsync("https://www.bungie.net/platform/User/GetBungieAccount/" + PlayerCharater.BungMembID + "/254/").Result;
                    if (res.IsSuccessStatusCode)
                    {
                        string resultContent = res.Content.ReadAsStringAsync().Result;
                        JObject json = JObject.Parse(resultContent);
                        dynamic jsonDe = JsonConvert.DeserializeObject(json.ToString());
                        for (int i = 0; i < 3; i++)
                        {
                            Console.WriteLine("Char ID: " + jsonDe.Response.destinyAccounts[0].characters[i].characterId);
                            PlayerCharater.Characters[i] = jsonDe.Response.destinyAccounts[0].characters[i].characterId;
                            if (jsonDe.Response.destinyAccounts[0].characters[i].gender.genderName == "Male")
                            {
                                Console.WriteLine("Char Class: " + jsonDe.Response.destinyAccounts[0].characters[i].race.raceNameMale + " - " + jsonDe.Response.destinyAccounts[0].characters[i].characterClass.className);
                                PlayerCharater.CharactersInfo[i] = jsonDe.Response.destinyAccounts[0].characters[i].race.raceNameMale + " - " + jsonDe.Response.destinyAccounts[0].characters[i].characterClass.className;
                            } else
                            {
                                Console.WriteLine("Char Class: " + jsonDe.Response.destinyAccounts[0].characters[i].race.raceNameFemale + " - " + jsonDe.Response.destinyAccounts[0].characters[i].characterClass.className);
                                PlayerCharater.CharactersInfo[i] = jsonDe.Response.destinyAccounts[0].characters[i].race.raceNameFemale + " - " + jsonDe.Response.destinyAccounts[0].characters[i].characterClass.className;
                            }                 
                            //PlayerCharater.Characters[i] = jsonDe.Response.destinyAccounts[0].characters[i].characterId;
                            //PlayerCharater.CharactersInfo[i] = jsonDe.Response.destinyAccounts[0].characters[i].characterClass.className;
                        }
                        PlayerCharater.DestMembID = jsonDe.Response.destinyAccounts[0].userInfo.membershipId;
                        Console.WriteLine("Got Destiny ID");
                        return "SUCCESS";
                    }
                    else
                    {
                        return "HTTP ERROR";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Unexpected Exception: " + ex.Message;
            }
        }
    }

    public static class PlayerCharater
    {
        public static string BungMembID { get; set; }
        public static string DestMembID { get; set; }
        public static String[] Characters = new string[4];
        public static String[] CharactersInfo = new string[4];
    }   
}