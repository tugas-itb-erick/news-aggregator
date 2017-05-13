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

namespace algo
{
   public class MainProgram
    {
        public static SortedListNews news = new SortedListNews();
        public static string[] rss;

        public static void loadRSS()
        {
            /*System.IO.StreamReader file = new System.IO.StreamReader("rss.txt");
            List<string> links = new List<string>();
            string line;*/
            List<string> links = new List<string>();
            links.Add("http://rss.detik.com/index.php/detikcom");
            links.Add("http://www.antaranews.com/rss/terkini");
            links.Add("http://rss.viva.co.id/get/all");
            links.Add("https://rss.tempo.co/index.php/teco/news/feed/");
            /*while ((line = file.ReadLine()) != null)
            {
                links.Add(line);
            }*/

            rss = links.ToArray();
        }

        /* I.S. string[] rss contains rss links */
       public static void loadListNews()
        {
            foreach (string rssLink in rss)
            {
                try
                {
                    // Parse XML from RSS link
                    int tes = 0;
                    XmlReader reader = XmlReader.Create(rssLink);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    /*XmlDocument doc = new XmlDocument();
                    doc.Load(rssLink);
                    XmlElement root = doc.DocumentElement;
                    XmlNodeList feed = root.SelectNodes("/channel/item");*/

                    // Iterate every item (news)
                    foreach (SyndicationItem item in feed.Items)
                    {
                        try
                        {
                            string title = item.Title.Text;
                            DateTimeOffset date = item.PublishDate;
                            string imageURL = "";
                            string contentURL = item.Links.First().Uri.ToString();
                            if (contentURL.Contains(News.TEMPO))
                            {
                                //imageURL = Regex.Match(item.Content.ToString(), "img src=(?:\"|\')?(?<imgSrc>[^>]*[^/].(?:jpg|bmp|gif|png))(?:\"|\')?").Groups[1].Value;
                            }
                            else
                            {
                                imageURL = Regex.Match(item.Summary.Text, "img src=(?:\"|\')?(?<imgSrc>[^>]*[^/].(?:jpg|bmp|gif|png))(?:\"|\')?").Groups[1].Value;
                            }

                            news.Add(new News(title, date, imageURL, contentURL));
                            tes++;
                            Console.WriteLine(tes);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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

            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            result = news.GetSearchResultWithBM("Ahok");
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(result[i].Item1 + " " + result[i].Item2 + "\n" + news.Get(result[i].Item1).GetContentSummary(result[i].Item2));
                Console.ReadKey();
            }
            Console.WriteLine("\n\n--DONE1--\n\n");
            Console.ReadKey();

            result.Clear();
            result = news.GetSearchResultWithRegex("Ahok");
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(result[i].Item1 + " " + result[i].Item2 + "\n" + news.Get(result[i].Item1).GetContentSummary(result[i].Item2));
                Console.ReadKey();
            }
            Console.WriteLine("\n\n--DONE1--\n\n");
            Console.ReadKey();
        }
    }
}

