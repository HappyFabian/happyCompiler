using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyIISProject
{
    public class webhandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            String path = context.Request.QueryString["Pathfile"];
            if (String.IsNullOrEmpty(path))
            {
                context.Response.Write("<div>\"Wrong Path\"</div>");
            }
            else
            {
                context.Response.Write("<div>"+ path +"</div>");
            }

        }

        public bool IsReusable { get { return true; } }
    }
}
