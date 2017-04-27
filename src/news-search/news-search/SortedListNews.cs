using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_search
{
    class SortedListNews
    {
        // Static instance
        public static SortedListNews news = new SortedListNews();

        private List<News> data;

        /*
         * Constructor.
         * Initialize new empty List of News.
         */
        public SortedListNews()
        {
            data = new List<News>();
        }

        /*
         * Add News x sorted by its date.
         */
        public void Add(News x)
        {
            int i = data.Count;
            while ((i > 0) && (data[i-1].getDate().CompareTo(x.getDate()) > 0))
            {
                i--;
            }
            data.Insert(i, x);
        }

        /*
         * Return News at index of i.
         */
        public News Get(int i)
        {
            return data[i];
        }

        /*
         * Return string representation of List of News.
         */
        override
        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            
            for(int i=0; i<data.Count; i++)
            {
                builder.AppendLine("[" + i + "]");
                builder.AppendLine(data[i].ToString() + "\n");
            }
           
            return builder.ToString();
        }

        public int Size()
        {
            return data.Count;
        }

    }
}
