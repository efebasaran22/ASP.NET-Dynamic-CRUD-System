using StajOdev.AppCode;
using System;
using System.Collections.Generic;
using System.Data;

namespace StajOdev.App_Code
{
	public class UserManager
	{
		private readonly DatabaseHelper _dbHelper;

		public UserManager()
		{
			_dbHelper = new DatabaseHelper();
		}

		public bool CreateUser(string firstName, string lastName, string email, string username, string password)
		{
			var values = new Dictionary<string, object>()
			{
				{"FirstName", firstName},
				{"LastName", lastName},
				{"Email", email},
				{"Username", username},
				{"PasswordHash", password}
            };

			try
			{
				_dbHelper.Insert("Users", values);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool ValidateUser(string username, string password)
		{
			string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

			var parameters = new Dictionary<string, object>()
			{
				{"Username", username},
				{"Password", password}
			};

			try
			{
				DataTable dt = _dbHelper.SelectWithParameters(query, parameters);
				if (dt.Rows.Count > 0)
				{
					int count = Convert.ToInt32(dt.Rows[0][0]);
					return count > 0;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool UpdateUser(int userId, string firstName, string lastName, string email)
		{
			var values = new Dictionary<string, object>()
			{
				{"FirstName", firstName},
				{"LastName", lastName},
				{"Email", email}
			};

			try
			{
				_dbHelper.Update("Users", userId, values);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool DeleteUser(int userId)
		{
			try
			{
				_dbHelper.Delete("Users", userId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
