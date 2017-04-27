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

        /*
         * Constructor.
         */ 
        public News()
        {
            title = "Untitled";
            imageURL = "error";
            contentURL = "error";
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
        }

        /* Return the news content from contentURL.
         * May throw exception.
         */
        private string GetContent()
        {
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

            return sb.ToString();//.Replace("\n\n", "");
        }

        public string SearchContentWithKMP(string pattern)
        {
            const int delta = 20;
            string text = GetContent();

            int indexFound = Matcher.kmpMatch(text, pattern);
            if (indexFound == Matcher.NOT_FOUND)
                return "";
            else
                return text.Substring(indexFound-delta, delta*2);
        }

        public string SearchContentWithBM(string pattern)
        {
            const int delta = 20;
            string text = GetContent();

            int indexFound = Matcher.bmMatch(text, pattern);
            if (indexFound == Matcher.NOT_FOUND)
                return "";
            else
                return text.Substring(indexFound - delta, delta * 2);
        }

        public string SearchContentWithRegex(string pattern)
        {
            const int delta = 20;
            string text = GetContent();

            int indexFound = Matcher.regexMatch(text, pattern);
            if (indexFound == Matcher.NOT_FOUND)
                return "";
            else
                return text.Substring(indexFound - delta, delta * 2);
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
        public String getTitle()
        {
            return title;
        }

        public void setTitle(String title)
        {
            this.title = title;
        }

        public DateTimeOffset getDate()
        {
            return date;
        }

        public void setDate(DateTimeOffset date)
        {
            this.date = date;
        }

        public String getImageURL()
        {
            return imageURL;
        }

        public void setImageURL(String imageURL)
        {
            this.imageURL = imageURL;
        }

        public String getContentURL()
        {
            return contentURL;
        }

        public void setContentURL(String contentURL)
        {
            this.contentURL = contentURL;
        }
    }
}
