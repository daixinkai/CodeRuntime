using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeRuntime.Common
{
    internal static class Tools
    {
        /// <summary>
        /// 得到当前应用程序中所有DLL文件
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetCurrentAssemblys()
        {

            string dir = HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : System.Web.Hosting.HostingEnvironment.MapPath("~/bin");

            var allFiles = Directory.GetFiles(dir, "*.dll");

            foreach (var item in allFiles)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(item);
                }
                catch
                {
                    continue;
                }
                yield return assembly;
            }
        }

    }
}
