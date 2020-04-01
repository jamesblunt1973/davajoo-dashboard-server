using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Models
{
	public class GetPrescriptionsParameter
	{
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public int? PatientId { get; set; }
		public int? PharmacyId { get; set; }
		public string Status { get; set; }
		public int UserId { get; set; }
		public int Page { get; set; }
	}
}
