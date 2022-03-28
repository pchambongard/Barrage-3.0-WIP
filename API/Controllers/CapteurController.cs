using Model;
using Microsoft.AspNetCore.Mvc;
using API.DAL;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CapteurController : ControllerBase
	{
		private readonly ILogger<CapteurController> logger;

		public CapteurController(ILogger<CapteurController> logger)
		{
			this.logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCapteur()
		{
			try
			{
				return Ok(await CapteurDAO.GetAllAsync().ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("idCapteur={idCapteur}")]
		public async Task<IActionResult> GetCapteurById(int idCapteur)
		{
			try
			{
				return Ok(await CapteurDAO.GetByIdAsync(idCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("idBarrage={idBarrage}")]
		public async Task<IActionResult> GetCapteurByBarrage(int idBarrage)
		{
			try
			{
				return Ok(await CapteurDAO.GetByBarrageAsync(idBarrage).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut("idBarrage={idBarrage}")]
		[HttpPost("idBarrage={idBarrage}")]
		public async Task<IActionResult> StoreCapteur([FromBody] Capteur capteur, int idBarrage)
		{
			try
			{
				return Ok(await CapteurDAO.StoreAsync(capteur, idBarrage));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}