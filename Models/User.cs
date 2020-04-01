namespace DavajooDashboardServer.Models
{
	public class Account
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Cell { get; set; }
		public bool IsPharmacy { get; set; }
		public string Token { get; set; }
	}
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Cell { get; set; }
		public string NationalCode { get; set; }
		public int TotalCount { get; set; }
	}
}
