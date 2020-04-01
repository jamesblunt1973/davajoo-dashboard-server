using DavajooDashboardServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Data
{
	public interface IAuthRepository
	{
		public Task<Account> Login(string userName, string password);
	}
}
