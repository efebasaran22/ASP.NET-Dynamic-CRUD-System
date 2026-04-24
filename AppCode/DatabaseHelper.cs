using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StajOdev.AppCode
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;

	public class DatabaseHelper
	{
		private readonly string _connectionString;

		public DatabaseHelper()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		}

		public void CreateTable(string tableName, Dictionary<string, string> columns)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				string query = $"CREATE TABLE {tableName} (Id INT IDENTITY(1,1) PRIMARY KEY, " +
					string.Join(",", columns.Select(c => $"{c.Key} {c.Value}")) + ")";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.ExecuteNonQuery();
			}
		}

		public void Insert(string tableName, Dictionary<string, object> values)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				string columnNames = string.Join(",", values.Keys);
				string paramNames = string.Join(",", values.Keys.Select(k => "@" + k));
				string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";
				SqlCommand cmd = new SqlCommand(query, conn);
				foreach (var pair in values)
					cmd.Parameters.AddWithValue("@" + pair.Key, pair.Value ?? DBNull.Value);
				cmd.ExecuteNonQuery();
			}
		}

		public void Update(string tableName, int id, Dictionary<string, object> values)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				string setClause = string.Join(",", values.Keys.Select(k => $"{k} = @{k}"));
				string query = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";
				SqlCommand cmd = new SqlCommand(query, conn);
				foreach (var pair in values)
					cmd.Parameters.AddWithValue("@" + pair.Key, pair.Value ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Id", id);
				cmd.ExecuteNonQuery();
			}
		}

		public void Delete(string tableName, int id)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				string query = $"DELETE FROM {tableName} WHERE Id = @Id";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@Id", id);
				cmd.ExecuteNonQuery();
			}
		}

		public DataTable SelectAll(string tableName)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				string query = $"SELECT * FROM {tableName}";
				SqlDataAdapter da = new SqlDataAdapter(query, conn);
				DataTable dt = new DataTable();
				da.Fill(dt);
				return dt;
			}
		}
		public DataTable SelectWithParameters(string query, Dictionary<string, object> parameters)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(query, conn);

				foreach (var param in parameters)
				{
					cmd.Parameters.AddWithValue("@" + param.Key, param.Value ?? DBNull.Value);
				}

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);
				return dt;
			}
		}

	}

}