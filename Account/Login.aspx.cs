using System;
using System.Web;
using System.Web.UI;
using StajOdev.App_Code; // Kendi UserManager sınıfını buradan çekiyoruz

namespace StajOdev.Account
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack && Request.Cookies["Username"] != null)
			{
				txtUsername.Text = Request.Cookies["Username"].Value;
				chkRememberMe.Checked = true;
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text.Trim();

			UserManager userManager = new UserManager();

			if (userManager.ValidateUser(username, password))
			{
				Session["Username"] = username;

				if (chkRememberMe.Checked)
				{
					HttpCookie cookie = new HttpCookie("Username", username);
					cookie.Expires = DateTime.Now.AddDays(7);
					Response.Cookies.Add(cookie);
				}

				Response.Redirect("~/Tables/CreateTable.aspx");
			}
			else
			{
				lblMessage.Text = "Kullanıcı adı veya şifre yanlış.";
			}
		}
	}
}
