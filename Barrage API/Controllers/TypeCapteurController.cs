using Microsoft.AspNetCore.Mvc;
using Barrage_Model;
using Barrage_API.DAL;

namespace Barrage_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TypeCapteurController : ControllerBase
	{
		private readonly ILogger<TypeCapteurController> logger;

		public TypeCapteurController(ILogger<TypeCapteurController> logger)
		{
			this.logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTypeCapteur()
		{
			try
			{
				return Ok(await TypeCapteurDAO.GetAllAsync().ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("idTypeCapteur={idTypeCapteur}")]
		public async Task<IActionResult> GetTypeCapteurById(int idTypeCapteur)
		{
			try
			{
				return Ok(await TypeCapteurDAO.GetByIdAsync(idTypeCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		[HttpPost]
		public async Task<IActionResult> StoreTypeCapteur([FromBody] TypeCapteur type)
		{
			try
			{
				return Ok(await TypeCapteurDAO.StoreAsync(type));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}