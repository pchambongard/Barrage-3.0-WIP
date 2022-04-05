using Microsoft.AspNetCore.Mvc;
using Barrage_API.DAL;
using Barrage_Model;

namespace Barrage_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BarrageController : ControllerBase
	{
		private readonly ILogger<BarrageController> logger;

		public BarrageController(ILogger<BarrageController> logger)
		{
			this.logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllBarrage()
		{
			try
			{
				return Ok(await BarrageDAO.GetAllAsync().ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("idBarrage={idBarrage}")]
		public async Task<IActionResult> GetBarrageById(int idBarrage)
		{
			try
			{
				return Ok(await BarrageDAO.GetByIdAsync(idBarrage).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		[HttpPost]
		public async Task<IActionResult> StoreBarrage([FromBody] Barrage barrage)
		{
			try
			{
				return Ok(await BarrageDAO.StoreAsync(barrage));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}