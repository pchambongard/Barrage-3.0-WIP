using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Barrage_Model;
using Service_Température.DAL;

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

		[HttpPost]
		public async Task<IActionResult> PostMesuresTempérature(HttpRequestMessage request)
		{
			try
			{
				logger.LogInformation("TEST");
				MesureTempérature mesure = new();
				var document = new XmlDocument();
				document.Load(request.Content!.ReadAsStreamAsync().Result);
				if (document.DocumentElement != null
					&& document.DocumentElement.SelectSingleNode("/Devices-Detail-Response/owd_DS18B20/Temperature") != null
					&& document.DocumentElement.SelectSingleNode("/Devices-Detail-Response/owd_DS18B20/Name") != null)
				{
					mesure.IdCapteur = 26;
					List<Capteur>? capteurs = new();
					HttpClient client = new();
					var requestCapteur = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:5254/api/Capteur"));
					var response = await client.SendAsync(requestCapteur).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						capteurs = await response.Content.ReadFromJsonAsync<List<Capteur>>().ConfigureAwait(false);
					}
					XmlNode nodeName = document.DocumentElement.SelectSingleNode("/Devices-Detail-Response/owd_DS18B20/ROMId")!;
					foreach (Capteur capteur in capteurs!)
					{
						if (capteur.Libellé == nodeName.Value)
						{
							mesure.IdCapteur = capteur.Id;
						}
					}
					XmlNode node = document.DocumentElement.SelectSingleNode("/Devices-Detail-Response/owd_DS18B20/Temperature")!;
					mesure.Gdh = DateTime.Now;
					mesure.Valeur = Convert.ToDecimal(node.Value);
					return Ok(MesureTempératureDAL.StoreAsync(mesure));
				}
				return Ok(null);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
