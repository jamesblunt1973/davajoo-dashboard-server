using DavajooDashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DavajooDashboardServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaciesController : ControllerBase
    {
        private readonly IRepository repo;
        private readonly ILogger logger;

        public PharmaciesController(IRepository repo, ILogger<PharmaciesController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<IActionResult> Get(string str, int page)
        {
            var userId = Helpers.GetUserId(HttpContext.User);
            if (userId != 13)
                return Forbid();
            var pharmacies = await repo.GetPharmacies(str, page);
            return Ok(pharmacies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPharmacyInfo(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}