using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Dictionary<string, string> languages = new Dictionary<string, string>
        {
            {"1", "en-US"},
            {"2", "uk-UA"}
        };

        Console.WriteLine("Please choose a language:");
        Console.WriteLine("1. English");
        Console.WriteLine("2. Ukrainian");
        Console.Write("Enter the language: ");
        string choice = Console.ReadLine();
        string lang = languages.GetValueOrDefault(choice);

        Dictionary<string, Dictionary<string, string>> localizations = new Dictionary<string, Dictionary<string, string>>
        {
            ["en-US"] = new Dictionary<string, string>
            {
                ["IsOnline"] = "is online now",
                ["JustNow"] = "was seen just now",
                ["LessThanAMinuteAgo"] = "was seen less than a minute ago",
                ["LatishAgo"] = "was seen a couple of minutes ago",
                ["AnHourAgo"] = "was seen an hour ago",
                ["Today"] = "was seen today",
                ["Yesterday"] = "was seen yesterday",
                ["ThisWeek"] = "was seen this week",
                ["LongTimeAgo"] = "was seen long, long time ago"
            },

            ["uk-UA"] = new Dictionary<string, string>
            {
                ["IsOnline"] = "у мережі",
                ["JustNow"] = "був_ла у мережі прямо зараз",
                ["LessThanAMinuteAgo"] = "був_ла у мережі менше хвилини тому",
                ["LatishAgo"] = "був_ла у мережі кілька хвилин тому",
                ["AnHourAgo"] = "був_ла у мережі годину тому",
                ["Today"] = "був_ла у мережі сьогодні",
                ["Yesterday"] = "був_ла у мережі вчора",
                ["ThisWeek"] = "був_ла у мережі цього тижні",
                ["LongTimeAgo"] = "був_ла у мережі доволі давно"
            }
        }

        List<Dictionary<string, string>> get_data(int offset)
        {
            string link = $"https://sef.podkolzin.consulting/api/users/lastSeen?offset={offset}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(link).Result;

            if (response.IsSuccessStatusCode)
            {
                Dictionary<string, List<Dictionary<string, string>>> data = response.Content.ReadAsAsync<Dictionary<string, List<Dictionary<string, string>>>>().Result;
                return data["data"];
            }
            else
            {
                return null;
            }
        }

        List<Dictionary<string, string>> get_all_data()
        {
            int offset = 0;
            List<Dictionary<string, string>> all_data = new List<Dictionary<string, string>>();

            while (true)
            {
                List<Dictionary<string, string>> data = get_data(offset);

                if (data == null || data.Count == 0)
                {
                    break;
                }

                all_data.AddRange(data);
                offset += data.Count;
            }

            return all_data;
        }

        
        (DateTime, string) parse_last_seen_date(string last_seen_str)
        {
            string timeInfo = last_seen_str.Substring(last_seen_str.Length - 6);
            last_seen_str = last_seen_str.Replace('T', ' ');

            if (last_seen_str.Contains('.'))
            {
                string[] time_parts = last_seen_str.Split('.');
                time_parts[1] = time_parts[1].Substring(0, Math.Min(6, time_parts[1].Length));
                last_seen_str = string.Join(".", time_parts);
            }

            last_seen_str = last_seen_str.Split('+')[0];
            return (DateTime.ParseExact(last_seen_str, "yyyy-MM-dd HH:mm:ss.ffffff", null), timeInfo);
        }

        DateTime adjust_timezone(DateTime last_seen, string timeInfo)
        {
            char sign = timeInfo[0];
            int hours = int.Parse(timeInfo.Substring(1, 2));

            if (sign == '+')
            {
                return last_seen.AddHours(-hours);
            }
            else if (sign == '-')
            {
                return last_seen.AddHours(hours);
            }

            return last_seen;
        }
    }
}