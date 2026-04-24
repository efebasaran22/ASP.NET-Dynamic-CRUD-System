using StajOdev.AppCode;
using StajOdev.Tables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StajOdev.App_Code
{
	public class TableManager
	{
		private readonly DatabaseHelper _dbHelper;

		public TableManager()
		{
			_dbHelper = new DatabaseHelper();
		}


		public void CreateTable(string tableName, Dictionary<string, string> columns)
		{
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Tablo adı boş olamaz.");

			if (columns == null || columns.Count == 0)
				throw new ArgumentException("Alanlar boş olamaz.");

			_dbHelper.CreateTable(tableName, columns);
		}


		public void InsertData(string tableName, Dictionary<string, object> values)
		{
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Tablo adı boş olamaz.");

			if (values == null || values.Count == 0)
				throw new ArgumentException("Veri boş olamaz.");

			_dbHelper.Insert(tableName, values);
		}

	
		public void UpdateData(string tableName, int id, Dictionary<string, object> values)
		{
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Tablo adı boş olamaz.");

			if (values == null || values.Count == 0)
				throw new ArgumentException("Veri boş olamaz.");

			_dbHelper.Update(tableName, id, values);
		}

		public void DeleteData(string tableName, int id)
		{
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Tablo adı boş olamaz.");

			_dbHelper.Delete(tableName, id);
		}

		public DataTable GetAllData(string tableName)
		{
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Tablo adı boş olamaz.");

			return _dbHelper.SelectAll(tableName);
		}
		public List<string> GetUserTables(string username)
		{
			// Basit örnek: veritabanından kullanıcının tablolarını çek
			// Biz demo amaçlı sys.tables'dan tüm tabloları alıyoruz
			var tables = new List<string>();

			using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				conn.Open();
				string query = "SELECT name FROM sys.tables WHERE name NOT LIKE 'sys%'";
				SqlCommand cmd = new SqlCommand(query, conn);
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					tables.Add(reader.GetString(0));
				}
			}
			return tables;
		}

		// Tablo sütunlarını getir (basit, Id hariç)
		public List<ColumnInfo> GetTableColumns(string tableName)
		{
			var columns = new List<ColumnInfo>();

			using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				conn.Open();
				string query = @"
                SELECT COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName AND COLUMN_NAME != 'Id'";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@TableName", tableName);

				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string colName = reader.GetString(0);
					string dataType = reader.GetString(1);

					string simplifiedType;

					switch (dataType)
					{
						case "int":
							simplifiedType = "int";
							break;
						case "decimal":
							simplifiedType = "decimal";
							break;
						case "datetime":
							simplifiedType = "datetime";
							break;
						default:
							simplifiedType = "varchar";
							break;
					}


					columns.Add(new ColumnInfo { ColumnName = colName, DataType = simplifiedType });
				}
			}
			return columns;
		}
		public void DropTable(string tableName)
		{
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Tablo adı boş olamaz.");

			using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				conn.Open();
				string query = $"DROP TABLE [{tableName}]";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.ExecuteNonQuery();
			}
		}
	}
}
