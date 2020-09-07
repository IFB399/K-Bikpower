using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class User
    {
		string id;
		string username;
		string email;
		string permission;
		string password;
		string salt;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		[JsonProperty(PropertyName = "username")]
		public string Username //must be unique
		{
			get { return username; }
			set { username = value; }
		}
		[JsonProperty(PropertyName = "email")]
		public string Email //must be unique
		{
			get { return email; }
			set { email = value; }
		}

		[JsonProperty(PropertyName = "permission")]
		public string Permission //must be unique
		{
			get { return permission; }
			set { permission = value; }
		}
		[JsonProperty(PropertyName = "password")]
		public string Password //must be unique
		{
			get { return password; }
			set { password = value; }
		}
		[JsonProperty(PropertyName = "salt")]
		public string Salt //must be unique
		{
			get { return salt; }
			set { salt = value; }
		}
	}
}
