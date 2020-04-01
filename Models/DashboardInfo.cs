using System;
using System.Collections.Generic;

namespace DavajooDashboardServer.Models
{
	public class DashboardInfo
	{
		public int Pharmacies { get; set; }
		public int Patients { get; set; }
		public int Prescriptions { get; set; }
		public int Delivered { get; set; }
		public IEnumerable<ActivePharmacy> ActivePharmacies { get; set; }
		public IEnumerable<DailyPrescription> DailyPrescriptions { get; set; }
	}

	public class ActivePharmacy
	{
		public string Name { get; set; }
		public int Count { get; set; }
	}

	public class DailyPrescription
	{
		public DateTime Date { get; set; }
		public int Total { get; set; }
		public int Delivered { get; set; }
		public int Canceled { get; set; }
	}
}
