using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class User
    {
		string id;
		string email;
		string role;
		string password;
		string salt;
		string firstName;
		string lastName;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		[JsonProperty(PropertyName = "email")]
		public string Email 
		{
			get { return email; }
			set { email = value; }
		}

		[JsonProperty(PropertyName = "role")]
		public string Role
		{
			get { return role; }
			set { role = value; }
		}
		[JsonProperty(PropertyName = "password")]
		public string Password
		{
			get { return password; }
			set { password = value; }
		}
		[JsonProperty(PropertyName = "salt")]
		public string Salt
		{
			get { return salt; }
			set { salt = value; }
		}
		[JsonProperty(PropertyName = "firstName")]
		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}
		[JsonProperty(PropertyName = "lastName")]
		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}
	}
}
