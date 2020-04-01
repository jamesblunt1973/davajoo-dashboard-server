using DavajooDashboardServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Data
{
	public class AuthRepository : IAuthRepository
	{
		private readonly IConfiguration configuration;
		private readonly ILogger<AuthRepository> logger;

		public AuthRepository(IConfiguration configuration, ILogger<AuthRepository> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
		}

		public async Task<Account> Login(string userName, string password)
		{
			var apiUrl = "https://pwa.davajoo.org/newApi/auth/local/";
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsJsonAsync(apiUrl, new
			{
				UserName = userName,
				Password = password,
				Version = "2.4.10"
			});
			var res = await response.Content.ReadAsAsync<Dictionary<string, object>>();
			Account account = null;
			try
			{
				response.EnsureSuccessStatusCode();

				account = new Account()
				{
					Cell = res["UserName"].ToString(),
					Id = Convert.ToInt32(res["ID"]),
					IsPharmacy = Convert.ToBoolean(res["Type"]),
					Name = res["Fname"] + " " + res["Lname"]
				};
			}
			catch(Exception ex)
			{
				logger.LogError(ex.GetExceptionMessage());
			}
			return account;
		}
	}
}
