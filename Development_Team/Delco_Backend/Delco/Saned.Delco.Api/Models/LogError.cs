using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saned.Delco.Api.Models
{
    public class ErrorSaver
    {
        public static void SaveError(string message)
        {


            string filePath = HttpContext.Current.Server.MapPath("~/Error/") + "Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + message + "<br/>" + Environment.NewLine + "StackTrace :" + message +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }


        }


    }
}