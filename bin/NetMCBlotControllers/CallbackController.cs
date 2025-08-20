using NETMVCBlot.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace NETMVCBlot.Controllers
{
    public class CallbackController : BaseController
    {
        protected override void Utility()
        {

        }

        [HttpPost]
        public ActionResult Download(string fileName)
        {
            if (!IBValidator.IsValidFileName(fileName))
                return new HttpNotFoundResult();

            var safeFileName = Regex.Replace(fileName, "[^a-zA-Z0-9-]", "");
            // CTSECISSUE:DirectoryTraversal
            return new FilePathResult(@"D:\wwwroot\reports\" + safeFileName, "application/pdf");
        }

        public String DownloadAsString(string fileName)
        {
            // CTSECISSUE:DirectoryTraversal
            var safeFileName = Regex.Replace(fileName, "[^a-zA-Z0-9-]", "");
            return System.IO.File.ReadAllText(@"D:\wwwroot\reports\" + safeFileName);
        }

        public JsonResult ExecuteProcess(string argument)
        {
            // CTSECISSUE: OSCommandInjection
            var safeArgument = Regex.Replace(argument, "[^a-zA-Z0-9-]", "");
            Process.Start("cmd.exe", "/C ping.exe " + safeArgument);
            return null;
        }
    }
}