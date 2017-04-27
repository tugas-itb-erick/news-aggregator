using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_search
{
    class SortedListNews
    {
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
            while ((i > 0) && (data[i-1].GetDate().CompareTo(x.GetDate()) < 0))
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

        /*
         * First int: index of data
         * Second int: index found, needed to get summary
         */
        public List<Tuple<int,int>> GetSearchResultWithKMP(string pattern)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for(int i=0; i<data.Count; i++)
            {
                int indexFound = data[i].SearchContentWithKMP(pattern.ToLower());
                if (indexFound != Matcher.NOT_FOUND)
                {
                    result.Add(new Tuple<int, int>(i, indexFound));
                }
            }

            return result;
        }

        /*
         * First int: index of data
         * Second int: index found, needed to get summary
         */
        public List<Tuple<int, int>> GetSearchResultWithBM(string pattern)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int i = 0; i < data.Count; i++)
            {
                int indexFound = data[i].SearchContentWithBM(pattern.ToLower());
                if (indexFound != Matcher.NOT_FOUND)
                {
                    result.Add(new Tuple<int, int>(i, indexFound));
                }
            }

            return result;
        }

        /*
         * First int: index of data
         * Second int: index found, needed to get summary
         */
        public List<Tuple<int, int>> GetSearchResultWithRegex(string pattern)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int i = 0; i < data.Count; i++)
            {
                int indexFound = data[i].SearchContentWithRegex(pattern.ToLower());
                if (indexFound != Matcher.NOT_FOUND)
                {
                    result.Add(new Tuple<int, int>(i, indexFound));
                }
            }

            return result;
        }

    }
}
