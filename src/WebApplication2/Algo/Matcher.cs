using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace algo
{
    public class Matcher
    {
        public const int NOT_FOUND = -1;

        /*
         * Match string with KMP
         */
        public static int kmpMatch(string text, string pattern)
        {
            if (pattern.Length == 0)
            {
                return NOT_FOUND;
            }

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
            if (pattern.Length == 0)
            {
                return NOT_FOUND;
            }

            int[] charTable = makeCharTable(pattern);
            int[] jumpTable = makeJumpTable(pattern);

            for (int i = pattern.Length - 1, j; i < text.Length;)
            {
                for (j = pattern.Length - 1; pattern[j] == text[i]; --i, --j)
                {
                    if (j == 0)
                    {
                        return i;
                    }
                }
                i += Math.Max(jumpTable[pattern.Length - 1 - j], charTable[text[i]]);
            }

            return NOT_FOUND;
        }

        private static int[] makeCharTable(string pattern)
        {
            const int ALPHABET_SIZE = Char.MaxValue + 1;
            int[] table = new int[ALPHABET_SIZE];
            for (int i = 0; i < table.Length; ++i)
            {
                table[i] = pattern.Length;
            }
            for (int i = 0; i < pattern.Length - 1; ++i)
            {
                table[pattern[i]] = pattern.Length - 1 - i;
            }
            return table;
        }

        private static int[] makeJumpTable(string pattern)
        {
            int[] table = new int[pattern.Length];
            int lastPrefixPosition = pattern.Length;
            for (int i = pattern.Length - 1; i >= 0; --i)
            {
                if (isPrefix(pattern, i + 1))
                {
                    lastPrefixPosition = i + 1;
                }
                table[pattern.Length - 1 - i] = lastPrefixPosition - i + pattern.Length - 1;
            }
            for (int i = 0; i < pattern.Length - 1; ++i)
            {
                int slen = suffixLength(pattern, i);
                table[slen] = pattern.Length - 1 - i + slen;
            }
            return table;
        }

        private static bool isPrefix(string pattern, int p)
        {
            for (int i = p, j = 0; i < pattern.Length; ++i, ++j)
            {
                if (pattern[i] != pattern[j])
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Returns the maximum length of the substring ends at p and is a suffix.
         */
        private static int suffixLength(string needle, int p)
        {
            int len = 0;
            for (int i = p, j = needle.Length - 1;
                     i >= 0 && needle[i] == needle[j]; --i, --j)
            {
                len += 1;
            }
            return len;
        }

        /*
         * Match string with Regex
         */
        public static int regexMatch(string text, string pattern)
        {
            if (pattern.Length == 0)
            {
                return NOT_FOUND;
            }

            Regex ptr = new Regex(pattern);
            Match match = ptr.Match(text);
            if (!match.Success)
                return NOT_FOUND;
            return match.Index;
        }
    }
}
