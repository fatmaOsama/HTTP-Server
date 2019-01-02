using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            // for each exception write its details associated with datetime 
            TextWriter tw = new StreamWriter(Configuration.LoggerFilePath, true);
            tw.WriteLine(ex.Message+" "+DateTime.Now);
            tw.Close();
        }
        public static void Log(string ex)
        {
            TextWriter tw = new StreamWriter(Configuration.LoggerFilePath, true);
            tw.WriteLine(ex + " " + DateTime.Now);
            tw.Close();
        }
    }
}
