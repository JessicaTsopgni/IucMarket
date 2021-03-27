using System;
using System.Text;

namespace IucMarket.Common
{
    public static class Extensions
    {
        public static string ToUcFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            s = s.ToLower();
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ToFirstCharToUpper(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToLower();
                char[] array = value.ToCharArray();
                if (array.Length >= 1)
                {
                    if (char.IsLower(array[0]))
                    {
                        array[0] = char.ToUpper(array[0]);
                    }
                }
                for (int i = 1; i < array.Length; i++)
                {
                    if (array[i - 1] == ' ')
                    {
                        if (char.IsLower(array[i]))
                        {
                            array[i] = char.ToUpper(array[i]);
                        }
                    }
                }
                return new string(array);
            }
            return null;
        }
        public static string ToKorM(this double value)
        {
            if (value > 1000000)
                return Math.Floor(value / 1000000).ToString() + "M";
            else if (value > 1000)
                return Math.Floor(value / 1000).ToString() + "K";
            else
                return value.ToString();
        }

        public static string EncodeToBase64(this string str)
        {
            var encodeTextBytes = Encoding.UTF8.GetBytes(str);
            var textResult = Convert.ToBase64String(encodeTextBytes);
            return textResult;
        }

        public static string DecodeToBase64(this string str)
        {
            var decodeTextBytes = Convert.FromBase64String(str);
            var textResult = Encoding.UTF8.GetString(decodeTextBytes);
            return textResult;
        }
        public static string ToRelativeDate(this DateTime date)
        {
            TimeSpan ts =DateTime.UtcNow.AddHours(1).Subtract(date);
            int intDays = ts.Days;
            int intHours = ts.Hours;
            int intMinutes = ts.Minutes;
            int intSeconds = ts.Seconds;

            if (intDays >= 365)
                return date.ToShortDateString();

            if (intDays == 30)
                return "A month ago";
            if (intDays > 30 && intDays < 60)
                return "More than a month ago";
            if (intDays >= 60)
                return string.Format("{0} months ago", Math.Round((decimal)intDays / 30));

            if (intDays == 7)
                return "A week ago";
            if (intDays > 7 && intDays < 14)
                return "More than a week ago";
            if (intDays >= 14)
                return string.Format("{0} weeks ago", Math.Round((decimal)intDays / 7));

            if (intDays == 1)
                return "A day ago";
            if (intDays == 2)
                return "Yesterday";
            if (intDays > 2)
                return string.Format("{0} days ago", intDays);

            if (intHours == 1)
                return "An hour ago";
            if (intHours > 0)
                return string.Format("{0} hours ago", intHours);

            if (intMinutes == 1)
                return "A minute ago";

            if (intMinutes > 0)
                return string.Format("{0} minutes ago", intMinutes);

            if (intSeconds == 1)
                return "A second ago";

            if (intSeconds > 0)
                return string.Format("{0} minutes ago", intSeconds);

            // let's handle future times..just in case
            if (intDays < 0)
                return string.Format("in {0} days", Math.Abs(intDays));

            if (intHours < 0)
                return string.Format("in {0} hours", Math.Abs(intHours));

            if (intMinutes < 0)
                return string.Format("in {0} minutes", Math.Abs(intMinutes));

            if (intSeconds < 0)
                return string.Format("in {0} seconds", Math.Abs(intSeconds));

            return date.ToShortDateString();
        }
    }
}
