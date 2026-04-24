using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using StajOdev.App_Code;

namespace StajOdev.Tables
{
	public partial class ListTables : System.Web.UI.Page
	{
		private TableManager _tableManager;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["Username"] == null)
			{
				Response.Redirect("~/Account/Login.aspx");
			}
			_tableManager = new TableManager();

			if (!IsPostBack)
			{
				LoadTables();
			}
		}

		private void LoadTables()
		{
			var tables = _tableManager.GetUserTables("demoUser"); 

			DataTable dt = new DataTable();
			dt.Columns.Add("TableName", typeof(string));

			foreach (var t in tables)
				dt.Rows.Add(t);

			gvTables.DataSource = dt;
			gvTables.DataBind();

			gvData.DataSource = null;
			gvData.DataBind();

			lblMessage.Text = "";
		}

		protected void gvTables_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "DeleteTable")
			{
				int index = Convert.ToInt32(e.CommandArgument);
				string tableName = gvTables.DataKeys[index].Value.ToString();

				if (tableName.ToLower() == "users")
				{
					lblMessage.ForeColor = System.Drawing.Color.Red;
					lblMessage.Text = "Bu tabloyu silemezsiniz.";
					return; 
				}

				try
				{
					_tableManager.DropTable(tableName);
					lblMessage.ForeColor = System.Drawing.Color.Green;
					lblMessage.Text = $"{tableName} başarıyla silindi.";

					LoadTables();
				}
				catch (Exception ex)
				{
					lblMessage.ForeColor = System.Drawing.Color.Red;
					lblMessage.Text = "Hata: " + ex.Message;
				}
			}
		}

		protected void gvTables_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (gvTables.SelectedIndex >= 0)
			{
				string selectedTable = gvTables.SelectedDataKey.Value.ToString();
				LoadTableData(selectedTable);
			}
		}

		private void LoadTableData(string tableName)
		{
			try
			{
				var dt = _tableManager.GetAllData(tableName);

				gvData.DataSource = dt;
				gvData.AutoGenerateColumns = true;
				gvData.DataBind();

				lblMessage.Text = "";
			}
			catch (Exception ex)
			{
				lblMessage.ForeColor = System.Drawing.Color.Red;
				lblMessage.Text = "Hata: " + ex.Message;
				gvData.DataSource = null;
				gvData.DataBind();
			}
		}

		protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			try
			{
				int id = Convert.ToInt32(gvData.DataKeys[e.RowIndex].Value);
				string tableName = gvTables.SelectedDataKey.Value.ToString();
				_tableManager.DeleteData(tableName, id);

				lblMessage.ForeColor = System.Drawing.Color.Green;
				lblMessage.Text = "Kayıt başarıyla silindi.";

				LoadTableData(tableName);
			}
			catch (Exception ex)
			{
				lblMessage.ForeColor = System.Drawing.Color.Red;
				lblMessage.Text = "Hata: " + ex.Message;
			}
		}
	}
}
