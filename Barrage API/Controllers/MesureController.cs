using Microsoft.AspNetCore.Mvc;
using Barrage_Model;
using Barrage_API.DAL;

namespace Barrage_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MesureController : ControllerBase
	{
		private readonly ILogger<MesureController> logger;

		public MesureController(ILogger<MesureController> logger)
		{
			this.logger = logger;
		}

		[HttpGet("idBarrage={idBarrage}")]
		public async Task<IActionResult> GetMesureBetweenByBarrage([FromBody] List<DateTime> dates, int idBarrage)
		{
			try
			{
				if (dates.Count == 0)
				{
					dates[1] = DateTime.Now;
				}
				return Ok(await MesureDAO.GetMesureBetweenByBarrage(dates[0], dates[1], idBarrage).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpGet("idBarrage={idBarrage}/idCapteur={idCapteur}")]
		public async Task<IActionResult> GetMesureBetweenByCapteur([FromBody] List<DateTime> dates, int idBarrage, int idCapteur)
		{
			try
			{
				if (dates.Count == 0)
				{
					dates[1] = DateTime.Now;
				}
				return Ok(await MesureDAO.GetMesureBetweenByCapteur(dates[0], dates[1], idBarrage, idCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpGet("last/idBarrage={idBarrage}/idCapteur={idCapteur}")]
		public async Task<IActionResult> GetLastMesureByCapteur(int idBarrage, int idCapteur)
		{
			try
			{
				return Ok(await MesureDAO.GetLastMesureByCapteur(idBarrage, idCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpPut]
		[HttpPost]
		public async Task<IActionResult> StoreMesure([FromBody] List<Mesure> mesures)
		{
			try
			{
				return Ok(await MesureDAO.StoreAsync(mesures));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
