using DavajooDashboardServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Data
{
	public interface IRepository
	{
		Task<IEnumerable<User>> GetUsers(string str, int page, int userId);
		Task<IEnumerable<Prescription>> GetPrescriptions(GetPrescriptionsParameter data);
		Task<IEnumerable<Pharmacy>> GetPharmacies(string str, int page);
		Task<DashboardInfo> GetInfo();
		Task<Account> GetUser(int id);
	}
}
