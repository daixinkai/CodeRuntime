#if Component
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeRuntime
{
    internal class DebugLogHttpModule : IHttpModule
    {
        public void Dispose()
        {

        }

        //private IAsyncResult OnEndRequestAsync(object sender, EventArgs e, AsyncCallback cb, object extraData)
        //{

        //}

        public event BeginEventHandler Test;

        public void Init(HttpApplication context)
        {
            context.EndRequest += Context_EndRequest;
            //context.AddOnEndRequestAsync(OnEndRequestAsync, new EndEventHandler)
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            DebugLog.Dispose();
        }
    }
}
#endif