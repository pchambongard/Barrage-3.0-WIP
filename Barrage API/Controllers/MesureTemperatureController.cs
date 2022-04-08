using Barrage_Model;
using Microsoft.AspNetCore.Mvc;
using Barrage_API.DAL;

namespace Service_Température.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MesureTemperatureController : ControllerBase
	{
		private readonly ILogger<MesureTemperatureController> logger;

		public MesureTemperatureController(ILogger<MesureTemperatureController> logger)
		{
			this.logger = logger;
		}

		[HttpGet("idCapteur={idCapteur}")]
		public async Task<IActionResult> GetMesureBetweenByCapteur([FromBody] List<DateTime> dates, int idCapteur)
		{
			try
			{
				logger.LogInformation(dates[0].ToString());
				logger.LogInformation(dates[1].ToString());
				if (dates.Count == 0)
				{
					dates[1] = DateTime.Now;
				}
				return Ok(await MesureTempératureDAO.GetMesureBetweenByCapteur(dates[0], dates[1], idCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpGet("last/idCapteur={idCapteur}")]
		public async Task<IActionResult> GetLastMesureByCapteur(int idCapteur)
		{
			try
			{
				return Ok(await MesureTempératureDAO.GetLastMesureByCapteur(idCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpPost]
		public async Task<IActionResult> PostMesuresTempérature([FromBody] List<MesureTempérature> mesures)
		{
			try
			{
				return Ok(await MesureTempératureDAO.StoreAsync(mesures));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
