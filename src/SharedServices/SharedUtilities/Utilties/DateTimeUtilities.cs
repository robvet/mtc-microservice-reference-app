using System;
using System.Collections.Generic;
using System.Text;

namespace SharedUtilities.Utilties
{
    public static class DateTimeUtilities
    {
        public static string ToFormattedDateTime(this DateTime dateAndTime, bool includeTime)
        {
            // Format: January 26th, 2006 at 2:19pm
            string dateSuffix = "<sup>th</sup>";
            switch (dateAndTime.Day)
            {
                case 1:
                case 21:
                case 31:
                    dateSuffix = "<sup>st</sup>";
                    break;
                case 2:
                case 22:
                    dateSuffix = "<sup>nd</sup>";
                    break;
                case 3:
                case 23:
                    dateSuffix = "<sup>rd</sup>";
                    break;
            }
            var dateFmt = string.Format("{0:MMMM} {1:%d}{2}, {3:yyyy} at {4:%h}:{5:mm}{6}",
                dateAndTime, dateAndTime, dateSuffix, dateAndTime, dateAndTime, dateAndTime,
                dateAndTime.ToString("tt").ToLower());
            if (!includeTime)
            {
                dateFmt = string.Format("{0:MMMM} {1:%d}{2}, {3:yyyy}",
                    dateAndTime, dateAndTime, dateSuffix, dateAndTime);
            }
            return dateFmt;
        }

        /// <summary>
        /// Where date/time HAS to be in the W3C date (ISO 8601) standard when reporting 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToW3CDate(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("s") + "Z";
        }

        /// <summary>
        /// Returns quarter for a datetime
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public static int GetQuarter(this DateTime fromDate)
        {
            return (fromDate.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Returns countdown to a target date
        /// </summary>
        /// <param name="value"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public static string ToDaysTil(this DateTime value, DateTime endDateTime)
        {
            var ts = new TimeSpan(endDateTime.Ticks - value.Ticks);
            var delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second" : ts.Seconds + " seconds";
            }
            if (delta < 120)
            {
                return "a minute";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month" : months + " months";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year" : years + " years";
        }
    }
}
