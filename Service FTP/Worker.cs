using Barrage_Model;
using Npgsql;
using Service_FTP.Data;
using System.Net;
using System.Net.Http.Json;

namespace Service_FTP
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

			TimerCallback timerCallback = new(DownloadFileFTP);
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
		public async void DownloadFileFTP(Object? state)
		{
			Thread.Sleep(TimeSpan.FromSeconds(30));

			WebClient client = new();
			HttpClient httpClient = new();

			client.Credentials = new NetworkCredential(InfosFTP.Username, InfosFTP.Password);
			_logger.LogInformation("Logs: user -> " + InfosFTP.Username + " password -> " + InfosFTP.Password);

			_logger.LogInformation("Download " + InfosFTP.FtpLimniPath + "To " + InfosFTP.DownloadLimniPath);
			client.DownloadFile(InfosFTP.FtpLimniPath, InfosFTP.DownloadLimniPath);
			_logger.LogInformation("Converting Data...");
			List<Mesure> mesuresLimni = await CsvToMesure.CsvConvertToMesureLimni(InfosFTP.DownloadLimniPath);
			_logger.LogInformation("Storing new data. " + DateTime.Now.ToString());
			var request = new HttpRequestMessage(HttpMethod.Post, new Uri(Infos.barrageAPI + "/Mesure/"))
			{
				Content = JsonContent.Create(mesuresLimni)
			};
			var response = await httpClient.SendAsync(request).ConfigureAwait(false);
			_logger.LogInformation("Done. Status:  " + response.StatusCode + ". " + DateTime.Now.ToString());

			_logger.LogInformation("Download " + InfosFTP.FtpPluvioPath + "To " + InfosFTP.DownloadPluvioPath);
			client.DownloadFile(InfosFTP.FtpPluvioPath, InfosFTP.DownloadPluvioPath);
			_logger.LogInformation("Converting Data...");
			List<Mesure> mesuresPluvio = await CsvToMesure.CsvConvertToMesurePluvio(InfosFTP.DownloadPluvioPath);
			_logger.LogInformation("Storing new data. " + DateTime.Now.ToString());
			request = new HttpRequestMessage(HttpMethod.Post, new Uri(Infos.barrageAPI + "/Mesure/"))
			{
				Content = JsonContent.Create(mesuresPluvio)
			};
			response = await httpClient.SendAsync(request).ConfigureAwait(false);
			_logger.LogInformation("Done. Status:  " + response.StatusCode + ". " + DateTime.Now.ToString());
		}
	}
}