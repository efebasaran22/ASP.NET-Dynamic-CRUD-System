using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using StajOdev.App_Code;

namespace StajOdev.Tables
{
	public partial class InsertData : System.Web.UI.Page
	{
		private const string SessionColumnsKey = "SelectedTableColumns";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["Username"] == null)
			{
				// Eğer session yoksa, login sayfasına yönlendir
				Response.Redirect("~/Account/Login.aspx");
			}
			if (!IsPostBack)
			{
				LoadUserTables();
			}
			else
			{
				RebuildInputControls();
			}
		}

		private void LoadUserTables()
		{
		
			TableManager manager = new TableManager();
			var tables = manager.GetUserTables("demoUser");

			ddlTables.DataSource = tables;
			ddlTables.DataBind();

			ddlTables.Items.Insert(0, new ListItem("-- Tablo Seç --", ""));
		}

		protected void ddlTables_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(ddlTables.SelectedValue))
			{
				LoadTableColumns(ddlTables.SelectedValue);
				RebuildInputControls();
			}
			else
			{
				phInputs.Controls.Clear();
				Session[SessionColumnsKey] = null;
			}
		}

		private void LoadTableColumns(string tableName)
		{
			TableManager manager = new TableManager();
			var columns = manager.GetTableColumns(tableName);
			Session[SessionColumnsKey] = columns;
		}

		private void RebuildInputControls()
		{
			phInputs.Controls.Clear();

			var columns = Session[SessionColumnsKey] as List<ColumnInfo>;
			if (columns == null)
				return;

			foreach (var col in columns)
			{
				if (col.ColumnName.Equals("Id", StringComparison.OrdinalIgnoreCase))
					continue; // ID alanı otomatik, gösterme

				Label lbl = new Label { Text = col.ColumnName + ": ", AssociatedControlID = "txt_" + col.ColumnName };
				TextBox txt = new TextBox { ID = "txt_" + col.ColumnName, CssClass = "input-medium" };

				phInputs.Controls.Add(lbl);
				phInputs.Controls.Add(txt);
				phInputs.Controls.Add(new LiteralControl("<br />"));
			}
		}

		protected void btnInsert_Click(object sender, EventArgs e)
		{
			var columns = Session[SessionColumnsKey] as List<ColumnInfo>;
			if (columns == null)
			{
				lblMessage.Text = "Lütfen tablo seçiniz.";
				return;
			}

			string tableName = ddlTables.SelectedValue;
			if (string.IsNullOrEmpty(tableName))
			{
				lblMessage.Text = "Lütfen tablo seçiniz.";
				return;
			}

			Dictionary<string, object> values = new Dictionary<string, object>();

			foreach (var col in columns)
			{
				if (col.ColumnName.Equals("Id", StringComparison.OrdinalIgnoreCase))
					continue;

				TextBox txt = phInputs.FindControl("txt_" + col.ColumnName) as TextBox;
				if (txt == null)
					continue;

				object val = txt.Text.Trim();

				if (string.IsNullOrEmpty(txt.Text.Trim()))
				{
					values[col.ColumnName] = DBNull.Value;
				}
				else
				{
					if (col.DataType == "int")
					{
						if (int.TryParse(txt.Text.Trim(), out int intVal))
							values[col.ColumnName] = intVal;
						else
						{
							lblMessage.Text = $"{col.ColumnName} alanına geçerli bir sayı girin.";
							return;
						}
					}
					else if (col.DataType == "decimal")
					{
						if (decimal.TryParse(txt.Text.Trim(), out decimal decVal))
							values[col.ColumnName] = decVal;
						else
						{
							lblMessage.Text = $"{col.ColumnName} alanına geçerli bir ondalık sayı girin.";
							return;
						}
					}
					else if (col.DataType == "datetime")
					{
						if (DateTime.TryParse(txt.Text.Trim(), out DateTime dtVal))
							values[col.ColumnName] = dtVal;
						else
						{
							lblMessage.Text = $"{col.ColumnName} alanına geçerli bir tarih girin.";
							return;
						}
					}
					else
					{
						values[col.ColumnName] = txt.Text.Trim();
					}
				}
			}

			try
			{
				TableManager manager = new TableManager();
				manager.InsertData(tableName, values);

				lblMessage.ForeColor = System.Drawing.Color.Green;
				lblMessage.Text = "Veri başarıyla eklendi.";


				RebuildInputControls();
			}
			catch (Exception ex)
			{
				lblMessage.ForeColor = System.Drawing.Color.Red;
				lblMessage.Text = "Hata: " + ex.Message;
			}
		}
	}


	public class ColumnInfo
	{
		public string ColumnName { get; set; }
		public string DataType { get; set; } 
	}
}
