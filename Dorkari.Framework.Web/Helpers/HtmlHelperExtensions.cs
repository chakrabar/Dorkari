using System.Web;
using System.Web.Mvc;

namespace Dorkari.Framework.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Inserts uncompressed version of specified minified file, when compile debug="true". 
        /// Will NOT work if uncompressed file is not present is same folder. Do NOT use ~ in relative path!
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="minifiedFilePath">Path to minified file e.g. @"/Scripts/targetjsORcss.min.js"</param>
        /// <returns></returns>
        public static string UseUncompressedInDebug(this HtmlHelper helper, string minifiedFilePath)
        {
            return (HttpContext.Current != null && HttpContext.Current.IsDebuggingEnabled == true)
                    ? minifiedFilePath.Replace(".min.", ".") : minifiedFilePath;
        }
    }
}