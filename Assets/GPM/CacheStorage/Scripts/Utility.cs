using System;
using System.Globalization;

namespace Gpm.CacheStorage.Util
{
    public static class Utility
    {
        public static string GetSizeText(long size)
        {

            double KB = 1024;
            double MB = 1024 * KB;
            double GB = 1024 * MB;

            double gb = (double)size / (double)GB;
            double mb = (double)size / (double)MB;
            double kb = (double)size / (double)KB;
            if (gb > 1)
            {
                return string.Format("{0:#.3}gb", gb);
            }
            else if (mb > 1)
            {
                return string.Format("{0:#.#}mb", mb);
            }
            else if (kb > 1)
            {
                return string.Format("{0:#.#}kb", kb);
            }

            return string.Format("{0:0.#}b", kb);
        }

        public static string GetTimeText(long ticks)
        {
            double year = ticks / (TimeSpan.TicksPerDay * 30 * 12);
            double month = ticks / (TimeSpan.TicksPerDay * 30);
            double week = ticks / (TimeSpan.TicksPerDay * 7);
            double day = ticks / TimeSpan.TicksPerDay;
            double hour = ticks / TimeSpan.TicksPerHour;
            double minute = ticks / TimeSpan.TicksPerMinute;
            double second = ticks / TimeSpan.TicksPerSecond;

            if (year > 0)
            {
                return string.Format("{0:#} year", year);
            }
            else if (month > 0)
            {
                return string.Format("{0:#} month", month);
            }
            else if (week > 0)
            {
                return string.Format("{0:#} week", week);
            }
            else if (day > 0)
            {
                return string.Format("{0:#} day", day);
            }
            else if (hour > 0)
            {
                second -= minute * 60;
                minute -= hour * 60;
                return string.Format("{0:0}:{1:00}:{2:00}", hour, minute, second);
            }
            else if (minute > 0)
            {
                second -= minute * 60;
                return string.Format("0:{0:00}:{1:00}", minute, second);
            }
            else
            {
                return string.Format("0:00:{0:00}", second);
            }
        }
    }
}