using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public static class Log
    {

        public static void Exception(Exception ex, string environmentPath)
        {
            var folderPath = Path.Combine(environmentPath, "StaticFiles");
            string filePath = Path.Combine(folderPath, "Logs.txt");
            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileModifiedDate = File.GetLastWriteTime(filePath);
            if ((DateTime.Now - fileModifiedDate).Days > 6)
            {
                File.WriteAllText(filePath, string.Empty);
            }
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
