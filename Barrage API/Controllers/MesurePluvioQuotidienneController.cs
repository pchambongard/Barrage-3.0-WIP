using Microsoft.AspNetCore.Mvc;
using Barrage_API.DAL;

namespace Barrage_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MesurePluvioQuotidienneController : ControllerBase
	{

		private readonly ILogger<MesurePluvioQuotidienneController> logger;

		public MesurePluvioQuotidienneController(ILogger<MesurePluvioQuotidienneController> logger)
		{
			this.logger = logger;
		}

		[HttpGet("idCapteur={idCapteur}")]
		public async Task<IActionResult> GetMesureBetweenByCapteur([FromBody] List<DateTime> dates, int idCapteur)
		{
			try
			{
				if (dates.Count == 0)
				{
					dates[1] = DateTime.Now;
				}
				return Ok(await MesurePluvioQuotidienneDAO.GetMesureBetweenByCapteur(dates[0], dates[1], idCapteur).ConfigureAwait(false));
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
