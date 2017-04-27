using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_search
{
    class News
    {
        private String title;
        private DateTimeOffset date;
        private String imageURL;
        private String contentURL;
        private String content;

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
        public News(String title, DateTimeOffset date, String imageURL, String contentURL)
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

            // Remove useless script
            doc.DocumentNode.Descendants()
                .Where(n => n.Name == "script" || n.Name == "style" || n.Name == "#comment" || n.Name == "li")
                .ToList()
                .ForEach(n => n.Remove());

            // Save HTML Content to String
            StringBuilder sb = new StringBuilder();
            foreach (HtmlTextNode node in doc.DocumentNode.SelectNodes("//text()"))
            {
                sb.Append(node.Text);
            }

            // delete all enters (CAN CAUSE NEWS TO BE DELETED!!!)
            content = sb.ToString().Replace("\n", "");

            content = content.ToLower();
        }

        public int SearchContentWithKMP(string pattern)
        {
            return Matcher.kmpMatch(content, pattern);
        }

        public int SearchContentWithBM(string pattern)
        {
            return Matcher.bmMatch(content, pattern);
        }

        public int SearchContentWithRegex(string pattern)
        {
            return Matcher.regexMatch(content, pattern);
        }

        /*
         * MASIH BISA SALAH
         * asumsi length berita > delta*2
         */
        public string GetContentSummary(int indexFound)
        {
            const int delta = 120;
            const string dots = "...";
            if (indexFound <= delta)
            {
                return dots + content.Substring(0, delta * 2) + dots;
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
            builder.Append("Link : " + contentURL);
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
