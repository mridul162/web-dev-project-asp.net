using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Furqan.User
{
    public partial class user : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Request.Url.AbsoluteUri.ToString().Contains("default.aspx"))
            {
                form1.Attributes.Add("class", "sub_page");
            }
            else
            {
                form1.Attributes.Remove("class");
                //Load the control
                Control sliderUserControl = (Control)Page.LoadControl("SliderUserControl.ascx");

                //Add the control to the panel
                pnlSliderUC.Controls.Add(sliderUserControl);
            }

            if (Session["userID"] != null)
            {
                lblLoginOrLogout.Text = "Logout";
            }

            else
            {
                lblLoginOrLogout.Text = "Login";
            }


        }

        protected void lblLoginOrLogout_Click(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("login.aspx");
            }

            else
            {
                Session.Abandon();
                Response.Redirect("login.aspx");
            }

        }

        protected void lbRegisterOrProfile_Click(object sender, EventArgs e)
        {
            if (Session["userID"] != null)
            {
                lbRegisterOrProfile.ToolTip = "User Profile";
                Response.Redirect("profile.aspx");
            }

            else
            {
                lbRegisterOrProfile.ToolTip = "User Registration";
                Response.Redirect("Reg.aspx");
            }
        }

    }
}