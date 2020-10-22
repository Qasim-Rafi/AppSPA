using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class Log
    {
        public static void Exception(Exception ex)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            string filePath = Directory.GetCurrentDirectory() + @"/StaticFiles/Log.txt";
            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            //var fileModifiedDate = File.GetLastWriteTime(filePath);
            //if (DateTime.Now.Date > fileModifiedDate.Date)
            //{
            //    File.WriteAllText(filePath, string.Empty);
            //}
            using (StreamWriter writer = new StreamWriter(filePath, true))// (File.Exists(filePath)) ? File.AppendText(filePath) : File.CreateText(filePath))
            {

                writer.WriteLine("---------------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }
                writer.WriteLine();
            }
        }

        public static string TraceMethod(string message, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            return methodName;
        }
    }
}
