using DavajooDashboardServer.Data;
using DavajooDashboardServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DavajooDashboardServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers(options =>
			{
				var policy = new AuthorizationPolicyBuilder()
								.RequireAuthenticatedUser()
								.Build();
				options.Filters.Add(new AuthorizeFilter(policy));
			});
			services.AddCors(options =>
			{
				options.AddPolicy("cors",
				builder =>
				{
					builder
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});
			// configure strongly typed settings objects
			var section = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(section);

			// configure jwt authentication
			var appSettings = section.Get<AppSettings>();
			var key = new SymmetricSecurityKey(
				Encoding.ASCII.GetBytes(appSettings.Token)
			);
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = key,
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});

			services.AddScoped<IAuthRepository, AuthRepository>();
			services.AddScoped<IRepository, Repository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			loggerFactory.AddFile("Logs/catalog-{Date}.txt");

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("cors");

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseDefaultFiles(); // must call before UseStaticFiles

			app.UseStaticFiles();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapFallbackToController(action: "Index", controller: "Home");
			});
		}
	}
}
