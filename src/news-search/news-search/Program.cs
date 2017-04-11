using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.ServiceModel.Syndication;

namespace news_search
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            /*XmlDocument doc = new XmlDocument();
            doc.Load("http://www.antaranews.com/rss/terkini");
            int i = 0;
            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string text = node.InnerText;
                Console.WriteLine(i++);
                Console.WriteLine(text);
            }*/

            XmlReader reader = XmlReader.Create("http://www.antaranews.com/rss/terkini");
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach(SyndicationItem item in feed.Items)
            {
                String title = item.Title.Text;
                String date = item.PublishDate.ToString();
                Console.WriteLine(title);
                Console.WriteLine(date);
            }

        }
    }
}
