using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace algo
{
    public class News
    {
        public const string DETIK = "detik";
        public const string ANTARA = "antara";
        public const string TEMPO = "tempo";
        public const string VIVA = "viva";

        private string title;
        private DateTimeOffset date;
        private string imageURL;
        private string contentURL;
        private string content;

        /*
         * Constructor.
         */ 
        public News()
        {
            title = "Untitled";
            imageURL = "error";
            contentURL = "error";
            content = "error";
        }

        /*
         * Constructor with parameters.
         */
        public News(string title, DateTimeOffset date, string imageURL, string contentURL)
        {
            this.title = title;
            this.date = date;
            this.imageURL = imageURL;
            this.contentURL = contentURL;

            string html;
            // Load HTML from content URL
            using (var client = new WebClient())
            {
                html = client.DownloadString(contentURL);
            }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            StringBuilder sb = new StringBuilder();
            if (contentURL.Contains(DETIK))
            {
                string text;
                try
                {
                    text = doc.DocumentNode.SelectSingleNode("//*[@class='detail_text']").InnerText;
                } catch (Exception e)
                {
                    text = doc.DocumentNode.SelectSingleNode("//*[@class='text_detail']").InnerText;
                }
                text = Regex.Replace(text, @"(<(.*)>)", "");
                text = Regex.Replace(text, @"\(.{2,3}/.{2,3}\)(\s*.*)*", "");
                text = Regex.Replace(text, @"\s\s", "");
                text = Regex.Replace(text, @"^\s+", "");

                sb.Append(text);
                content = sb.ToString();
            }
            else if (contentURL.Contains(ANTARA))
            {
                string text = doc.DocumentNode.SelectSingleNode("//*[@id='content_news']").InnerText;
                text = Regex.Replace(text, @"(<(.*)>)", "");
                text = Regex.Replace(text, @"Editor:(\s*.*)*", "");
                text = Regex.Replace(text, @"\s\s", "");

                sb.Append(text);
                content = sb.ToString();
            }
            else if (contentURL.Contains(VIVA))
            {
                string text = doc.DocumentNode.SelectSingleNode("//*[@itemprop='description']").InnerText;
                //text = Regex.Replace(text, @"â?'A", "");
                text = text.Remove(10, 5);

                sb.Append(text);
                content = sb.ToString();
            }
            else if (contentURL.Contains(TEMPO))
            {
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//p"))
                {
                    sb.Append(node.InnerText);
                }
                content = sb.ToString();

                content = Regex.Replace(content, @"Disclaimer(\s*.*)*", "");
                content = Regex.Replace(content, @"(?<=\.)[^.]*$", "");
                this.title = Regex.Replace(this.title, @"\s\s", "");

                string imageClass = doc.DocumentNode.SelectSingleNode("//*[@class='single-img']").InnerHtml;
                //Console.WriteLine(imageClass);
                this.imageURL = Regex.Match(imageClass, "img src=(?:\"|\')(.*)(?:\"|\') alt?", RegexOptions.RightToLeft).Groups[1].Value;
            }
            else
            {
                // Remove useless script
                doc.DocumentNode.Descendants()
                    .Where(n => n.Name == "script" || n.Name == "style" || n.Name == "#comment" || n.Name == "li")
                    .ToList()
                    .ForEach(n => n.Remove());

                // Save HTML Content to String
                foreach (HtmlTextNode node in doc.DocumentNode.SelectNodes("//text()"))
                {
                    sb.Append(node.Text);
                }

                content = sb.ToString();
            }
            
            //Console.WriteLine(ToString());
            //Console.WriteLine(getGoogleMapsLink(getLocation()));
            //Console.ReadKey();
        }

        public String getLocation()
        {
            if (contentURL.Contains(VIVA))
            {
                return "";
            }

            // const int SPACE_ASCII = 32;
            const int LIMIT = 20;
            StringBuilder str = new StringBuilder();

            int i = 0;
            if (contentURL.Contains(TEMPO))
            {
                i = 10;
            }

            while ((i < LIMIT) && (content[i] != ' ')){
                str.Append(content[i]);
                i++;
            }

            return str.ToString();
        }

        public String getGoogleMapsLink(String location)
        {
            if (location.Length <= 1)
            {
                return "";
            }

            const String GOOGLE_MAPS_LINK = "https://www.google.co.id/maps/place/";

            return GOOGLE_MAPS_LINK + location;
        }

        public int SearchContentWithKMP(string pattern)
        {
            return Matcher.kmpMatch(content.ToLower(), pattern.ToLower());
        }

        public int SearchContentWithBM(string pattern)
        {
            return Matcher.bmMatch(content.ToLower(), pattern.ToLower());
        }

        public int SearchContentWithRegex(string pattern)
        {
            return Matcher.regexMatch(content.ToLower(), pattern.ToLower());
        }

        /*
         * MASIH BISA SALAH
         * asumsi length berita > delta*2
         */
        public string GetContentSummary(int indexFound)
        {
            const int delta = 120;
            const string dots = "...";
            if (content.Length <= delta * 2)
            {
                return content;
            }

            if (indexFound <= delta)
            {
                return content.Substring(0, delta * 2) + dots;
            }
            else if (indexFound + delta > content.Length)
            {
                return dots + content.Substring(content.Length - delta * 2 - 1, delta * 2);
            }
            else
            {
                return dots + content.Substring(indexFound - delta, delta * 2) + dots;
            }
        }

        /*
         * Return string representation of News.
         */
        override
        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Title: " + title);
            builder.AppendLine("Date : " + date);
            builder.AppendLine("Image: " + imageURL);
            builder.AppendLine("Link : " + contentURL);
            builder.Append("Content :\n" + content);
            return builder.ToString();
        }

        /* Getter and Setter */
        public string GetContent()
        {
            return content;
        }

        public string GetTitle()
        {
            return title;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public DateTimeOffset GetDate()
        {
            return date;
        }

        public void SetDate(DateTimeOffset date)
        {
            this.date = date;
        }

        public string GetImageURL()
        {
            return imageURL;
        }

        public void SetImageURL(string imageURL)
        {
            this.imageURL = imageURL;
        }

        public string GetContentURL()
        {
            return contentURL;
        }

        public void SetContentURL(string contentURL)
        {
            this.contentURL = contentURL;
        }
    }
}
