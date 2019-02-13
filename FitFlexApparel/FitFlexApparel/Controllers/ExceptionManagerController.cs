using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitFlexApparel.Controllers
{
    public class ExceptionManagerController : Controller
    {
        
        public static void infoMessage(string _message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLogs.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + _message + "\n\n\n\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void writeErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " | " + ex.Source.ToString().Trim() + " | " + ex.StackTrace.ToString().Trim() + " | " + ex.Message.ToString().Trim() + "\n\n\n\n");
                if(ex.InnerException != null)
                    sw.WriteLine(DateTime.Now.ToString() + " | " + ex.Source.ToString().Trim() + " | " + ex.InnerException.Message.ToString().Trim() + " | " + ex.InnerException.StackTrace.ToString().Trim() + "\n\n\n\n");

                sw.Flush();
                sw.Close();
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }
    }
}