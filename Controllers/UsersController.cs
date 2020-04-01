using DavajooDashboardServer.Data;
using DavajooDashboardServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository repo;
        private readonly ILogger logger;

        public UsersController(IRepository repo, ILogger<UsersController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<IActionResult> Get(string str, int page)
        {
            var userId = Helpers.GetUserId(HttpContext.User);
            var users = await repo.GetUsers(str, page, userId);
            return Ok(users);
        }
    }
}