using Microsoft.AspNetCore.Mvc;
using Barrage_Model;
using Barrage_API.DAL;

namespace Barrage_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TypeBarrageController : ControllerBase
	{
		private readonly ILogger<TypeBarrageController> logger;

		public TypeBarrageController(ILogger<TypeBarrageController> logger)
		{
			this.logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTypeBarrage()
		{
			try
			{
				return Ok(await TypeBarrageDAO.GetAllAsync().ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("idTypeBarrage={idTypeBarrage}")]
		public async Task<IActionResult> GetTypeBarrageById(int idTypeBarrage)
		{
			try
			{
				return Ok(await TypeBarrageDAO.GetByIdAsync(idTypeBarrage).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		[HttpPost]
		public async Task<IActionResult> StoreTypeBarrage([FromBody] TypeBarrage type)
		{
			try
			{
				return Ok(await TypeBarrageDAO.StoreAsync(type));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
