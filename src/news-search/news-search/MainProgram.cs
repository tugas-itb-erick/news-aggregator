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
                try
                {
                    // Parse XML from RSS link
                    int tes = 0;
                    XmlReader reader = XmlReader.Create(rssLink);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    // Iterate every item (news)
                    foreach (SyndicationItem item in feed.Items)
                    {
                        String title = item.Title.Text;
                        DateTimeOffset date = item.PublishDate;
                        String imageURL = Regex.Match(item.Summary.Text, "img src=(?:\"|\')?(?<imgSrc>[^>]*[^/].(?:jpg|bmp|gif|png))(?:\"|\')?").Groups[1].Value;
                        String contentURL = item.Links.First().Uri.ToString();

                        news.Add(new news_search.News(title, date, imageURL, contentURL));
                        tes++;
                        Console.WriteLine(tes);
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

            /*News n = new News("tes", new DateTimeOffset(), "", "http://news.detik.com/read/2017/04/26/153056/3484462/727/kota-mandiri-masa-depan-hadirkan-cbd-dan-hunian-di-satu-kawasan");
            int x = n.SearchContentWithBM("Tangerang");
            Console.WriteLine(x + n.GetContentSummary(x));
            news.Add(n);
            */

            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            result = news.GetSearchResultWithBM("Ahok");
            for(int i=0; i<result.Count; i++)
            {
                Console.WriteLine(result[i].Item1 + " " + result[i].Item2 + " " + news.Get(result[i].Item1).GetContentSummary(result[i].Item2));
            }
            Console.ReadKey();
            Console.WriteLine(1024);
            Console.ReadKey();
        }
    }
}
