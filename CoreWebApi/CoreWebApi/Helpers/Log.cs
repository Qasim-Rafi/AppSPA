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

            string filePath = Directory.GetCurrentDirectory() + @"/StaticFiles/Log.txt";
            if (!File.Exists(filePath))
            {
                //File.Create(filePath).Dispose();
            }
            //var fileModifiedDate = File.GetLastWriteTime(filePath);
            //if (DateTime.Now.Date > fileModifiedDate.Date)
            //{
            //    File.WriteAllText(filePath, string.Empty);
            //}
            using (StreamWriter writer = (File.Exists(filePath)) ? File.AppendText(filePath) : File.CreateText(filePath))
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
                writer.WriteLine();
            }
        }
    }
}
