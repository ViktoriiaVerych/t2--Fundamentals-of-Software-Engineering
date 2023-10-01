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
            } 
        }
    }
}