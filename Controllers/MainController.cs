using DavajooDashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IRepository repo;
        private readonly ILogger logger;

        public MainController(IRepository repo, ILogger<MainController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<IActionResult> Get()
        {
            var userId = Helpers.GetUserId(HttpContext.User);
            return Ok(await repo.GetInfo());
        }
    }
}