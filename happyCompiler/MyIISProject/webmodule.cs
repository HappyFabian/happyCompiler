using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyIISProject
{
    public class webmodule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(OnPreRequestHandlerExectute);
        }

        private void OnPreRequestHandlerExectute(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication) sender;
            HttpRequest request = app.Context.Request;
        }

        public void Dispose()
        {
            
        }
    }
}
