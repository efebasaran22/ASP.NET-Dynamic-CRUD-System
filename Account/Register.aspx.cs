using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StajOdev.Account
{
	public partial class Register : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnRegister_Click(object sender, EventArgs e)
		{
			string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string query = "INSERT INTO Users (FirstName, LastName, Email, Username, Password) VALUES (@FirstName, @LastName, @Email, @Username, @Password)";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
				cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
				cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
				cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
				cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
				conn.Open();
				cmd.ExecuteNonQuery();
				lblMessage.Text = "Kayıt başarılı!";
			}
		}
	}
}