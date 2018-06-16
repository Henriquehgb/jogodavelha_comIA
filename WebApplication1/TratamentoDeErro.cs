using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class TratamentoDeErro : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication app)
        {
            app.Error += new System.EventHandler(OnError);
        }

        public void OnError(object obj, EventArgs args)
        {
            // At this point we have information about the error
            HttpContext ctx = HttpContext.Current;

            string url = ctx.Request.RawUrl;

            //string url = ctx.Server.MapPath("/Shared/erros.aspx");
            Exception exception = ctx.Server.GetLastError();


            string erroInfo = ("Erro:" + exception.Message);

            if (ctx.Session != null)
            {
                if (exception.InnerException != null)
                {
                    if (exception.InnerException.ToString().Contains("DropDownList"))
                    {
                        string nomeEstrutura = exception.InnerException.ToString();
                        nomeEstrutura = nomeEstrutura.Substring(nomeEstrutura.IndexOf("DropDownList") + 12, nomeEstrutura.Length - (nomeEstrutura.IndexOf("DropDownList") + 12));
                        string mensagem = nomeEstrutura.Substring(0, nomeEstrutura.IndexOf(" ") - 1) + " é inválido.";

                        ctx.Session["Erro"] = "";
                        ctx.Session["InnerException"] = mensagem;

                        ctx.Response.Redirect("../Shared/erros.aspx");
                        ctx.Server.ClearError();
                    }
                    else
                    {
                        ctx.Session["Erro"] = (string)erroInfo;
                        ctx.Session["InnerException"] = exception.InnerException.ToString();

                        ctx.Response.Redirect("PaginaDeTratamento.aspx");
                        ctx.Server.ClearError();
                    }
                }

            }
            else
            {
                ctx.Response.Redirect("PaginaDeTratamento.aspx?Erro=" + erroInfo);
                ctx.Server.ClearError();
            }
        }
        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}