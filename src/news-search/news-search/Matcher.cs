using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace news_search
{
    class Matcher
    {
        public const int NOT_FOUND = -1;

        /*
         * Match string with KMP
         */
        public static int kmpMatch(string text, string pattern)
        {
            int[] fail = computeFail(pattern);
            int i = 0, j = 0;

            while (i < text.Length)
            {
                if (pattern[j] == text[i])
                {
                    if (j == pattern.Length - 1)
                        return i - pattern.Length + 1;
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = fail[j - 1];
                }
                else
                {
                    i++;
                }
            }

            return NOT_FOUND;
        }

        private static int[] computeFail(string pattern)
        {
            int[] fail = new int[pattern.Length];
            fail[0] = 0;

            int i = 1;
            int j = 0;
            while (i < pattern.Length)
            {
                if (pattern[j] == pattern[i])
                {
                    fail[i++] = ++j;
                }
                else if (j > 0)
                {
                    j = fail[j - 1];
                }
                else
                {
                    fail[i++] = 0;
                }
            }

            return fail;
        }

        /*
         * Match string with BM
         */
        public static int bmMatch(string text, string pattern)
        {
            int[] last = buildLast(pattern);
            int n = text.Length;
            int m = pattern.Length;
            int i = m - 1;

            if (i > n - 1)
            {
                return -1;
            }

            int j = m - 1;
            do
            {
                if (pattern[j] == text[i])
                {
                    if (j == 0)
                    {
                        return i;
                    }
                    else
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    int loc = last[text[i]];
                    i += m - Math.Min(j, 1 + loc);
                    j = m - 1;
                }
            } while (i <= n - 1);

            return NOT_FOUND;
        }

        private static int[] buildLast(string pattern)
        {
            const int ASCII_SET = 128;
            int[] last = new int[ASCII_SET];

            for (int i = 0; i < ASCII_SET; i++)
            {
                last[i] = -1;
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                last[pattern[i]] = i;
            }

            return last;
        }

        /*
         * Match string with Regex
         */
        public static int regexMatch(string text, string pattern)
        {
            Regex ptr = new Regex(pattern);
            Match match = ptr.Match(text);
            if (!match.Success)
                return NOT_FOUND;
            return match.Index;
        }
    }
}
