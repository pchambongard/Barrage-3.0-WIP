using Barrage_Model;
using System.Xml;
using System.Net.Http.Json;
using System.Globalization;

namespace Service_Temp
{
    public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;

		public Worker(ILogger<Worker> logger)
		{
			_logger = logger;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{	
			WaitStart();

			TimerCallback timerCallback = new(RequestData);
			Timer timer = new(timerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(1000, stoppingToken);
			}
		}
		public void WaitStart()
		{
			DateTime start = new((DateTime.Now.Ticks + TimeSpan.FromMinutes(5).Ticks - 1) / TimeSpan.FromMinutes(5).Ticks * TimeSpan.FromMinutes(5).Ticks, DateTime.Now.Kind);
			TimeSpan wait = start - DateTime.Now;

			_logger.LogInformation("\tTime: " + DateTime.Now.ToString() + "\n\tStart in: " + wait.ToString() + "\n\tAt: " + start.ToString()); ;

			Thread.Sleep(wait);
		}
		public async void RequestData(Object? Data)
		{
			HttpClient client = new();
			List<Capteur>? capteurs = new();
			List<MesureTempérature> mesures = new();
			DateTime start = DateTime.Now.AddDays(-1).ToLocalTime();
			DateTime end = DateTime.Now.ToLocalTime();

			var requestCapteur = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:5254/api/Capteur"));
			var responseCapteur = await client.SendAsync(requestCapteur).ConfigureAwait(false);
			if (responseCapteur.IsSuccessStatusCode)
			{
				capteurs = await responseCapteur.Content.ReadFromJsonAsync<List<Capteur>>().ConfigureAwait(false);
			}

			var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://www.thermotrack-webserve.com/API/get_api.php?method=equip_list&api_key=532c-16a171e&format=xml"));
			var response = await client.SendAsync(request).ConfigureAwait(false);
			if (response.IsSuccessStatusCode && response.Content != null)
			{
				var document = new XmlDocument();
				document.Load(response.Content!.ReadAsStreamAsync().Result);
				XmlNodeList? nodes = document.SelectNodes("Detail-Response/data/Equipment");
				if (nodes != null)
				{
					_logger.LogInformation("There is : " + nodes!.Count.ToString() + "sondes found");
					foreach (XmlNode node in nodes!)
					{
						var uri = new Uri("https://www.thermotrack-webserve.com/API/get_api.php?method=equip_data&api_key=532c-16a171e&equip_id="
							+ node.ChildNodes[0]!.InnerText + "&date_start=" + start.ToString("yyyy'-'MM'-'dd") + "&date_end=" + end.ToString("yyyy'-'MM'-'dd") + "&format=xml");
						var requestData = new HttpRequestMessage(HttpMethod.Get, uri);
						var responseData = await client.SendAsync(requestData).ConfigureAwait(false);

						if (responseData.IsSuccessStatusCode && response.Content != null)
						{
							var doc = new XmlDocument();
							doc.Load(responseData.Content!.ReadAsStreamAsync().Result);
							XmlNodeList? dataNodes = doc.SelectNodes("Detail-Response/data/Record");

							if (dataNodes != null)
							{
								_logger.LogInformation(" There is : " + dataNodes!.Count.ToString() + "records for this sonde.");
								foreach (XmlNode dataNode in dataNodes!)
								{

									if (dataNode.ChildNodes != null && dataNode.ChildNodes.Count >= 2)
									{
										int id = FindIdCapteur(node.ChildNodes[1]!.InnerText, capteurs!);

										if (id != -1)
										{
											mesures.Add(new(id, DateTime.ParseExact(dataNode.ChildNodes[0]!.InnerText, "yyyy-MM-dd HH:mm:ss", null), Convert.ToDecimal(dataNode.ChildNodes[1]!.InnerText, CultureInfo.InvariantCulture)));
										}
									}
								}
							}
						}
					}
				}
				if (mesures.Count > 0)
				{
					_logger.LogInformation(mesures.Count.ToString());
					var requestPost = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5254/api/MesureTemperature/")
					{
						Content = JsonContent.Create(mesures)
					};
					var responsePost = await client.SendAsync(requestPost).ConfigureAwait(false);
					_logger.LogInformation("Done. Status:  " + responsePost.StatusCode + ". " + DateTime.Now.ToString());
				}
				else
				{
					_logger.LogInformation("No mesures: Check logs to see where it went wrong.");
				}
			}

		}

		public static int FindIdCapteur(string capteur, List<Capteur> capteurs)
		{
			foreach (Capteur c in capteurs)
			{
				if (capteur == c.Libellé)
				{
					return c.Id;
				}
			}
			return -1;
		}

    }
}