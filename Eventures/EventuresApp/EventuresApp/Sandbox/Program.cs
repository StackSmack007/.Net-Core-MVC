namespace Sandbox
{
    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Parser;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Program
    {
        private static string urlAddressFormat = "https://fun.dir.bg/vic_open.php?id={0}";
        private static int jokeNumber = 0;
        private static string regexPatternCategory = @"class=""active"">(.+)<\/a><\/p>";
        private static string regexPatternJoke = @"<div id=""newsbody"">((\n*.+?)*?\n*)<\/div>";
        private static StringBuilder sb = new StringBuilder();
        private static object lockObj = new object();

        static void Main(string[] args)
        {
            System.Text.EncodingProvider ppp;
            ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            List<Task> tasks = new List<Task>();
            using (WebClient client = new WebClient { Encoding = Encoding.GetEncoding("windows-1251") })
            {
                for (int i = 1; i < 3000; i++)
                {
                    RetrieveJoke(i, client);
                    if (i % 15 == 0)
                    {
                        using (var filestream = new FileStream(".\\jokes.txt", FileMode.OpenOrCreate))
                        {
                            filestream.Write(Encoding.UTF8.GetBytes(sb.ToString().Trim()));
                        }
                    }
                }
            }
                    }

        private static async void RetrieveJoke(int number, WebClient client)
        {
            var parser = new HtmlParser();
            string urlAddress = string.Format(urlAddressFormat, number);
            string response = client.DownloadString(urlAddress);
            IHtmlDocument document = parser.ParseDocument(response);

            string joke = document.QuerySelector("#newsbody")?.TextContent?.Trim();
            string category = document.QuerySelector(".tag-links-left a")?.TextContent?.Trim();

            if (!string.IsNullOrEmpty(joke) && !string.IsNullOrEmpty(category))
            {
                //lock (lockObj)
                //{
                sb.AppendLine($"Joke number: {++jokeNumber} ({number})");
                sb.AppendLine($"Category: {category}");
                sb.AppendLine();
                sb.AppendLine(joke);
                sb.AppendLine(new string('-', 15));
                Console.WriteLine("jokeNumber: "+jokeNumber);
                // }
            }
        }
    }
}