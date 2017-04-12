using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace news_search
{
    class Program
    {
        static string[] loadRSS()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../rss.txt");
            List<string> rss = new List<string>();
            string line;

            while((line = file.ReadLine()) != null)
            {
                rss.Add(line);
            }

            return rss.ToArray();
        }

        static void Main(string[] args)
        {
            string[] rss = loadRSS();
            foreach(string rssLink in rss)
            {
                Console.Write(rssLink);

                // Parse XML from RSS link
                XmlReader reader = XmlReader.Create(rssLink);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();

                // Iterate every item (news)
                foreach (SyndicationItem item in feed.Items)
                {
                    String title = item.Title.Text;
                    String link = item.Links.First().Uri.ToString(); // TODO: Add try-catch block
                    String date = item.PublishDate.ToString();
                    Console.WriteLine("Date: " + date);
                    Console.WriteLine("Title: " + title);
                    Console.WriteLine("Link: " + link);
                    Console.WriteLine("Content: ");

                    // Parse HTML Page
                    String html;
                    using (var client = new WebClient())
                    {
                        html = client.DownloadString(link); // TODO: Add try-catch block
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    doc.DocumentNode.Descendants()
                        .Where(n => n.Name == "script" || n.Name == "style" || n.Name == "#comment" || n.Name == "li")
                        .ToList()
                        .ForEach(n => n.Remove());

                    // Save HTML Content to String
                    StringBuilder sb = new StringBuilder();
                    foreach (HtmlTextNode node in doc.DocumentNode.SelectNodes("//text()"))
                    {
                        sb.AppendLine(node.Text);
                    }
                    String content = sb.ToString().Replace("\n", ""); // TODO: fix \n replacement

                    Console.WriteLine(content);


                    Console.WriteLine();
                    Console.ReadKey();
                }
            }

        }
    }
}
