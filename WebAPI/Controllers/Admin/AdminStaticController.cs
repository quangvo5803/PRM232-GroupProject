using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    public partial class AdminController : ControllerBase
    {
        [HttpGet("Statistic")]
        public async Task<IActionResult> GetStatistic()
        {
            try
            {

                var data = await _facadeService.Static.GetStatisticAsync();

                if (data == null)
                {
                    return BadRequest("No data available");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
