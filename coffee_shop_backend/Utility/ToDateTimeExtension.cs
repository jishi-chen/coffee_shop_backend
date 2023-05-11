using System.Globalization;

namespace coffee_shop_backend.Utility
{
    public static class DateTimeTextExtension
    {
        private static string _dateTimeFormat = "yyyy-MM-dd";
        public static DateTime? ToStartDateTime(this string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return null;
            else
                return DateTime.Parse(date + " 00:00:00");
        }
        public static DateTime? ToEndDateTime(this string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return null;
            else
                return DateTime.Parse(date + " 23:59:59");
        }
        public static string ToDateTimeFormatString(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(_dateTimeFormat);
            else
                return null;
        }
        public static string ToDateTimeFormatString(this DateTime date)
        {
            return date.ToString(_dateTimeFormat);
        }
        /// <summary>
        /// 西元年轉民國年
        /// </summary>
        public static string ToChineseDateString(this DateTime date)
        {
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            return date.ToString("yyy-MM-dd", culture);
        }
        /// <summary>
        /// 西元年轉民國年
        /// </summary>
        public static string ToChineseDateString(this DateTime? date)
        {
            if (!date.HasValue)
                return null;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            return date.Value.ToString("yyy-MM-dd", culture);
        }
        /// <summary>
        /// 民國年轉西元年
        /// </summary>
        public static DateTime ToBCDateString(this DateTime? date)
        {
            string dateString = date.Value.ToString();
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            return DateTime.Parse(dateString, culture);
        }
    }
}
