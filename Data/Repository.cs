using Dapper;
using DavajooDashboardServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Data
{
	public class Repository : IRepository
	{
		private readonly IConfiguration configuration;
		private readonly ILogger<Repository> logger;

		public Repository(IConfiguration configuration, ILogger<Repository> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
		}

		public async Task<DashboardInfo> GetInfo()
		{
			string connectionString = configuration.GetConnectionString("Default");

			var commandText = "dash.GetDashboardInfo";
			// var param = new { userId };
			var command = new CommandDefinition(commandText, commandType: CommandType.StoredProcedure);

			DashboardInfo info;
			try
			{
				using SqlConnection connection = new SqlConnection(connectionString);
				var result = connection.QueryMultiple(command);
				info = new DashboardInfo();
				info.Pharmacies = await result.ReadFirstAsync<int>();
				info.Patients = await result.ReadFirstAsync<int>();
				info.Prescriptions = await result.ReadFirstAsync<int>();
				info.Delivered = await result.ReadFirstAsync<int>();
				info.ActivePharmacies = await result.ReadAsync<ActivePharmacy>();
				info.DailyPrescriptions = await result.ReadAsync<DailyPrescription>();
			}
			catch (Exception ex)
			{
				logger.LogError(ex.GetExceptionMessage());
				info = null;
			}

			return info;
		}

		public async Task<IEnumerable<User>> GetUsers(string str, int page, int userId)
		{
			string connectionString = configuration.GetConnectionString("Default");

			var commandText = "dash.GetUsers";
			var param = new { str, page, userId };
			var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

			IEnumerable<User> users;
			try
			{
				using SqlConnection connection = new SqlConnection(connectionString);
				users = await connection.QueryAsync<User>(command);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.GetExceptionMessage());
				users = null;
			}

			return users;
		}

		public async Task<IEnumerable<Prescription>> GetPrescriptions(GetPrescriptionsParameter data)
		{
			string connectionString = configuration.GetConnectionString("Default");

			var commandText = "dash.GetPrescriptions";
			var command = new CommandDefinition(commandText, data, commandType: CommandType.StoredProcedure);

			IEnumerable<Prescription> prescriptions;
			try
			{
				using SqlConnection connection = new SqlConnection(connectionString);
				var dic = new Dictionary<int, Prescription>();
				var result = await connection.QueryAsync<Prescription, Patient, Image, PrescriptionPharmacy, Prescription>(command,
					(prescription, patient, image, pharmacy) =>
					{
						if (!dic.TryGetValue(prescription.Id, out Prescription p))
						{
							p = prescription;
							p.Patient = patient;
							p.Images = new List<Image>();
							p.Pharmacies = new List<PrescriptionPharmacy>();
							dic.Add(p.Id, p);
						}
						if (image != null && !p.Images.Any(a => a.Id == image.Id))
							p.Images.Add(image);
						if (pharmacy != null && !p.Pharmacies.Any(a => a.Id == pharmacy.Id))
							p.Pharmacies.Add(pharmacy);
						return p;
					});
				prescriptions = result.Distinct();
			}
			catch (Exception ex)
			{
				logger.LogError(ex.GetExceptionMessage());
				prescriptions = null;
			}

			return prescriptions;
		}

		public async Task<IEnumerable<Pharmacy>> GetPharmacies(string str, int page)
		{
			string connectionString = configuration.GetConnectionString("Default");

			var commandText = "dash.GetPharmacies";
			var param = new { str, page };
			var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

			IEnumerable<Pharmacy> pharmacies;
			try
			{
				using SqlConnection connection = new SqlConnection(connectionString);
				pharmacies = await connection.QueryAsync<Pharmacy>(command);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.GetExceptionMessage());
				pharmacies = null;
			}

			return pharmacies;
		}

		public async Task<Account> GetUser(int id)
		{
			string connectionString = configuration.GetConnectionString("Default");

			var commandText = "SELECT Id, Fname + ' ' + Lname AS Name, UserName AS Cell, Type AS IsPharmacy FROM [dbo].[User] WHERE Id = @Id";
			var param = new { id };
			var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

			Account account;
			try
			{
				using SqlConnection connection = new SqlConnection(connectionString);
				account = await connection.QuerySingleOrDefaultAsync<Account>(command);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.GetExceptionMessage());
				account = null;
			}

			return account;
		}
	}
}
