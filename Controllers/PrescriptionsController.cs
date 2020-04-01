using DavajooDashboardServer.Data;
using DavajooDashboardServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IRepository repo;
        private readonly ILogger logger;

        public PrescriptionsController(IRepository repo, ILogger<PrescriptionsController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<IActionResult> Post(GetPrescriptionsParameter data)
        {
            data.UserId = Helpers.GetUserId(HttpContext.User);
            var prescriptions = await repo.GetPrescriptions(data);
            return Ok(prescriptions);
        }
    }
}