using DavajooDashboardServer.Data;
using DavajooDashboardServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
	[AllowAnonymous]
	public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
		private readonly IRepository dataRepo;
		private readonly IConfiguration configuration;

		public AuthController(IAuthRepository repo, IRepository dataRepo, IConfiguration configuration)
        {
            this.repo = repo;
			this.dataRepo = dataRepo;
			this.configuration = configuration;
		}

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginParameter data)
        {
            var account = await repo.Login(data.UserName, data.Password);
            if (account == null)
                return Unauthorized();
			if (!account.IsPharmacy)
				return Forbid();
			account.Token = GenerateJwtToken(account);
            return Ok(account);
        }

		[HttpGet("check-user")]
		public async Task<IActionResult> CheckUser()
		{
			var nameId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(nameId))
				return Ok(false);
			var userId = Convert.ToInt32(nameId);
			var account = await dataRepo.GetUser(userId);
			if (account == null)
				return NotFound();
			account.Token = GenerateJwtToken(account);
			return Ok(account);
		}

		public string GenerateJwtToken(Account account)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
				new Claim(ClaimTypes.Name, account.Name)
			};
			// configure strongly typed settings objects
			var section = configuration.GetSection("AppSettings");

			// configure jwt authentication
			var appSettings = section.Get<AppSettings>();
			var key = new SymmetricSecurityKey(
				Encoding.ASCII.GetBytes(appSettings.Token)
			);
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

	}
}