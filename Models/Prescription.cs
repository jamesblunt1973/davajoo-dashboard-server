using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DavajooDashboardServer.Models
{
	[DebuggerDisplay("Id = {Id}")]
	public class Prescription
	{
		public int Id { get; set; }
		public DateTime CreateDate { get; set; }
		public string Description { get; set; }
		public string Otc { get; set; }
		public int Status { get; set; }
		public bool IsPwa { get; set; }
		public int TotalCount { get; set; }

		public Patient Patient { get; set; }
		public List<Image> Images { get; set; }
		public List<PrescriptionPharmacy> Pharmacies { get; set; }
	}

	public class Patient
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string NationalCode { get; set; }
	}

	public class Image
	{
		public int Id { get; set; }
		public string FileName { get; set; }
	}

	public class PrescriptionPharmacy
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
