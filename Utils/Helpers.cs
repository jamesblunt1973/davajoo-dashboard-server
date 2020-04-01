using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace DavajooDashboardServer
{
	public static class Helpers
	{
		internal static int GetUserId(ClaimsPrincipal user)
		{
			var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				throw new Exception("Invalid Token");
			return Convert.ToInt32(userId);
		}
	}
}
