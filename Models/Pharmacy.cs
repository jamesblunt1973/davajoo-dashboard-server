using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Models
{
	public class Pharmacy
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Cell { get; set; }
		public string Phone { get; set; }
		public string Hix { get; set; }
		public string Gln { get; set; }
		public string Address { get; set; }
		public int TotalCount { get; set; }
	}
}
