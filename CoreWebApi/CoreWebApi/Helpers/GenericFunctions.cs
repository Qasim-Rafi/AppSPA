using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public static class GenericFunctions
    {
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
        public static string NotificationDescription(string[] ValuesToShow, string From)
        {
            var details = string.Join(" ", ValuesToShow);
            return $"{ details }{ Environment.NewLine }From: { From }";
        }
        public static decimal CalculatePercentage(int count, int total)
        {
            return decimal.Round((decimal)count / total * 100);
        }
        public static decimal CalculatePercentage(double first, double second)
        {
            return decimal.Round(Convert.ToDecimal(first / second * 100));
        }
        public static string CheckDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                var exist = date.Contains("00:00:00");
                if (exist)
                {
                    return Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                }
                else
                {
                    return date;
                }
            }
            else
            {
                return "";
            }

        }
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings != null)
            {
                if (settings is ExpandoObject)
                    return ((IDictionary<string, object>)settings).ContainsKey(name);

                return settings.GetType().GetProperty(name) != null;
            }
            else
                return false;
        }
        
        private static T GetNext<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return default(T);
            }
        }

        private static T GetPrevious<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }
        public static string GetDescription(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }
    }
    public class ApplyDocumentVendorExtensions : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            var pathsToRemove = swaggerDoc.Paths
            .Where(pathItem => pathItem.Key.Contains("/WeatherForecast")
            //|| pathItem.Key.Contains("api/Leaves") 
            || pathItem.Key.Contains("api/Values"))
            .ToList();

            foreach (var item in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(item.Key);
            }
        }
    }
}