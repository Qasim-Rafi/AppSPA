using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class DateFormat
    {
        public static DateTime ToDate(DateTime dateString)
        {
            //bool isValid = DateTime.TryParseExact(dateString.ToString(), "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime dt);
            var day = dateString.Day;
            var month = dateString.Month;
            var year = dateString.Year;
            var hour = dateString.Hour;
            var minute = dateString.Minute;
            var second = dateString.Second;
            var tt = dateString.Hour > 12 ? "PM" : "AM";
            //string[] formats = { "dd/MM/yyyy", "dd/MM/yyyy h:mm:ss", "dd/MM/yyyy hh:mm:ss", "MM/dd/yyyy", "MM/dd/yyyy hh:mm tt", "MM/d/yyyy hh:mm:ss", "MM/dd/yyyy hh:mm:ss", "M/d/yyyy hh:mm:ss", "MM/dd/yyyy h:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd hh:mm:ss", "yyyy/MM/dd h:mm:ss", "yyyy/M/d hh:mm:ss", "yyyy/MM/d hh:mm:ss" };
            //var dt = DateTime.ParseExact(dateString.ToString(), formats, new CultureInfo("en-US"), DateTimeStyles.None);
            //DateTime dt = DateTime.ParseExact(dateString, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            var dt = new DateTime(year, month, day, hour, minute, second).ToString("dd/MM/yyyy");
            return Convert.ToDateTime(dt);// Convert.ToDateTime($"{day}-{month}-{year} {hour}:{minute} {tt}");
        }
        public static DateTime ToDateTime(string dateString)
        {
            string[] formats = { "dd/MM/yyyy", "dd/MM/yyyy h:mm:ss tt", "dd/MM/yyyy hh:mm:ss tt", "MM/dd/yyyy", "MM/dd/yyyy hh:mm tt", "MM/dd/yyyy hh:mm:ss tt", "yyyy/MM/dd", "yyyy/MM/dd hh:mm:ss tt", "yyyy/MM/dd hh:mm:ss", "yyyy/MM/dd hh:mm tt" };
            var dt = DateTime.ParseExact(dateString.ToString(), formats, new CultureInfo("en-US"), DateTimeStyles.None);
            return Convert.ToDateTime(dt.ToString("dd/MM/yyyy hh:mm tt"));
        }
    }

}
