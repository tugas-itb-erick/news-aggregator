using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections;

namespace news_search
{
    class MainProgram
    {
        static SortedListNews news = new SortedListNews();
        static string[] rss;

        static void loadRSS()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../rss.txt");
            List<string> links = new List<string>();
            string line;

            while((line = file.ReadLine()) != null)
            {
                links.Add(line);
            }

            rss = links.ToArray();
        }

        /* I.S. string[] rss contains rss links */
        static void loadListNews()
        {
            foreach (string rssLink in rss)
            {
                // Parse XML from RSS link
                XmlReader reader = XmlReader.Create(rssLink);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();

                // Iterate every item (news)
                foreach (SyndicationItem item in feed.Items)
                {
                    try
                    {
                        String title = item.Title.Text;
                        DateTimeOffset date = item.PublishDate;
                        String imageURL = Regex.Match(item.Summary.Text, "img src=(?:\"|\')?(?<imgSrc>[^>]*[^/].(?:jpg|bmp|gif|png))(?:\"|\')?").Groups[1].Value;
                        String contentURL = item.Links.First().Uri.ToString();

                        news.Add(new news_search.News(title, date, imageURL, contentURL));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        static void init()
        {
            loadRSS();
            loadListNews();
        }

        static void Main(string[] args)
        {
            init();

            /*for(int i=0; i<news.Size(); i++)
            {
                News n = news.Get(i);
            }*/

            Console.WriteLine(news.ToString());
            Console.ReadKey();
        }
    }
}
