using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class PaginaDeTratamento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Erro"] != null)
            {
                LabelErro.Text = Session["Erro"].ToString();
                LabelnnerException.Text = Session["InnerException"].ToString();
            }
            else
            {
                LabelErro.Text = Request.QueryString["Erro"];
                LabelnnerException.Visible = false;
            }
        }
    }
}