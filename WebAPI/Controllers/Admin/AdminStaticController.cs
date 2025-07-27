using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    public partial class AdminController : ControllerBase
    {
        [HttpGet("Statistic")]
        public async Task<IActionResult> GetStatistic()
        {
            var data = await _facadeService.Static.GetStatisticAsync();
            return Ok(data);
        }

    }
}
