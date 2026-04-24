using System;
using System.Collections.Generic;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using StajOdev.App_Code;

namespace StajOdev.Tables
{
	
	public partial class CreateTable : System.Web.UI.Page
	{
		private const string SessionFieldsKey = "TableFields";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["Username"] == null)
			{
				// Eğer session yoksa, login sayfasına yönlendir
				Response.Redirect("~/Account/Login.aspx");
			}
			if (!IsPostBack)
			{
				var fields = new List<FieldInfo> { new FieldInfo { Name = "", Type = "VARCHAR(50)" } };
				Session[SessionFieldsKey] = fields;
			}

			RebuildFieldControls();
		}

		protected void btnAddField_Click(object sender, EventArgs e)
		{
			UpdateFieldsFromControls();

			var fields = Session[SessionFieldsKey] as List<FieldInfo>;
			fields.Add(new FieldInfo { Name = "", Type = "VARCHAR(50)" });

			Session[SessionFieldsKey] = fields;

			RebuildFieldControls();
		}

		protected void btnCreateTable_Click(object sender, EventArgs e)
		{
			string tableName = txtTableName.Text.Trim();

			if (string.IsNullOrEmpty(tableName))
			{
				lblMessage.Text = "Lütfen tablo adını girin!";
				lblMessage.ForeColor = System.Drawing.Color.Red;
				return;
			}

			UpdateFieldsFromControls();

			var fields = Session[SessionFieldsKey] as List<FieldInfo>;

			if (fields == null || fields.Count == 0)
			{
				lblMessage.Text = "En az bir alan eklemelisiniz!";
				lblMessage.ForeColor = System.Drawing.Color.Red;
				return;
			}

			var columns = new Dictionary<string, string>();

			foreach (var field in fields)
			{
				if (string.IsNullOrWhiteSpace(field.Name))
				{
					lblMessage.Text = "Alan adları boş olamaz!";
					lblMessage.ForeColor = System.Drawing.Color.Red;
					return;
				}

				
				string[] allowedTypes = { "VARCHAR(50)", "INT", "DECIMAL(18,2)", "DATETIME" };
				if (Array.IndexOf(allowedTypes, field.Type) < 0)
				{
					lblMessage.Text = $"Geçersiz alan tipi: {field.Type}";
					lblMessage.ForeColor = System.Drawing.Color.Red;
					return;
				}

				if (!columns.ContainsKey(field.Name))
					columns.Add(field.Name, field.Type);
				else
				{
					lblMessage.Text = $"Alan adı tekrar edemez: {field.Name}";
					lblMessage.ForeColor = System.Drawing.Color.Red;
					return;
				}
			}

			try
			{
				TableManager manager = new TableManager();
				manager.CreateTable(tableName, columns);

				lblMessage.Text = "Tablo başarıyla oluşturuldu!";
				lblMessage.ForeColor = System.Drawing.Color.Green;

				
				Session[SessionFieldsKey] = new List<FieldInfo> { new FieldInfo { Name = "", Type = "VARCHAR(50)" } };
				txtTableName.Text = "";
				RebuildFieldControls();
			}
			catch (Exception ex)
			{
				lblMessage.Text = "Hata: " + ex.Message;
				lblMessage.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void RebuildFieldControls()
		{
			phFields.Controls.Clear();

			var fields = Session[SessionFieldsKey] as List<FieldInfo>;

			if (fields == null)
				return;

			for (int i = 0; i < fields.Count; i++)
			{
				Panel panel = new Panel { CssClass = "field-row" };

				TextBox txtName = new TextBox
				{
					ID = "txtName" + i,
					Text = fields[i].Name,
					CssClass = "input-medium"
				};
				txtName.Attributes.Add("placeholder", "Alan Adı");


				DropDownList ddlType = new DropDownList
				{
					ID = "ddlType" + i,
					CssClass = "input-medium"
				};

				ddlType.Items.Add("VARCHAR(50)");
				ddlType.Items.Add("INT");
				ddlType.Items.Add("DECIMAL(18,2)");
				ddlType.Items.Add("DATETIME");
				ddlType.SelectedValue = fields[i].Type;

				panel.Controls.Add(txtName);
				panel.Controls.Add(ddlType);

				phFields.Controls.Add(panel);
			}
		}

		private void UpdateFieldsFromControls()
		{
			var fields = new List<FieldInfo>();

			foreach (Control ctrl in phFields.Controls)
			{
				if (ctrl is Panel pnl)
				{
					TextBox txtName = null;
					DropDownList ddlType = null;

					foreach (Control child in pnl.Controls)
					{
						if (child is TextBox t) txtName = t;
						else if (child is DropDownList d) ddlType = d;
					}

					if (txtName != null && ddlType != null)
					{
						fields.Add(new FieldInfo
						{
							Name = txtName.Text.Trim(),
							Type = ddlType.SelectedValue
						});
					}
				}
			}

			Session[SessionFieldsKey] = fields;
		}

		[Serializable]
		private class FieldInfo
		{
			public string Name { get; set; }
			public string Type { get; set; }
		}
	}
}
