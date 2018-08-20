using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Strings
{
    public static class Format
    {
        public static String formatDate(DateTime dt)
        {
            return dt.ToShortDateString();
        }

        public static String formatDateTime(DateTime dt)
        {
            return dt.ToShortDateString() + " " + dt.ToLongTimeString();
        }

        public static String formatDateTimeConecto(DateTime dt)
        {
            return string.Format("{0}{1}{2}{3}{4}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
        }


        public static String formatNumber(ulong input)
        {
            return String.Format("{0:N0}", input);
        }

        public static String formatPrice(string price)
        {
            return String.Format("{0:N0},-", price);
        }

        public static String formatDoublePrice(double price)
        {
            return String.Format("{0:0.00}", price);
        }


        public static int formatStrToInt(string price)
        {
            double amount;
            double.TryParse(price.Replace(".", ","), out amount);

            return (int)amount;
        }

        public static string formatStrToKr(string price)
        {
            double amount;
            double.TryParse(price.Replace(".", ","), out amount);

            return string.Format("{0},-", (int)amount);
        }

        public static string formatStrToKroner(string price)
        {
            double amount;
            double.TryParse(price.Replace(".", ","), out amount);

            return string.Format("{0} kroner", (int)amount);
        }

        public static string formatBytes(ulong bytes, bool show)
        {
            if (bytes > 1000000000)
            {
                return String.Format("{0:0,0}{1}", ((double)bytes / 1000000), (show) ? " MB" : "");
            }
            else if (bytes > 1000000)
            {
                return String.Format("{0:0.0}{1}", ((double)bytes / 1000000), (show) ? " MB" : "");
            }
            else if (bytes > 1000)
            {
                return String.Format("{0:0}{1}", ((double)bytes / 1000), (show) ? " KB" : "");
            }
            else
            {
                return String.Format("{0}{1}", bytes, (show) ? " bytes" : "");
            }
        }

        public static string formatBytesInGB(ulong bytes, bool showDecimal, bool show)
        {
            if (showDecimal)
                return String.Format("{0:0.0}{1}", ((double)bytes / 1000000000), (show) ? " GB" : "");
            else
                return String.Format("{0:0}{1}", ((double)bytes / 1000000000), (show) ? " GB" : "");
        }


        public static double StringToDouble(String number)
        {
            string str = number.Replace(".", ",");

            return Convert.ToDouble(str);
        }

        public static string Right(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || maxLength <= 0)
            {
                return string.Empty;
            }

            if (maxLength < text.Length)
            {
                return text.Substring(text.Length - maxLength);
            }

            return text;
        }

        public static string AddLeadingZero(string number, int maxLength)
        {
            if (string.IsNullOrEmpty(number) || maxLength <= 0)
            {
                return string.Empty;
            }

            if (number.Length >= maxLength)
            {
                return number;
            }

            return number.PadLeft(maxLength, '0');
        }


        public static byte[] ToUTF8Bytes(String str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public static String ToString(byte[] utf8)
        {
            return System.Text.Encoding.UTF8.GetString(utf8);
        }

        public static String ToString(byte[] data, String encoding)
        {
            return System.Text.Encoding.GetEncoding(encoding).GetString(data);
        }

        public static String ToURLString(String src)
        {
            return System.Web.HttpUtility.UrlEncode(src);
        }
    }
}
