using Barrage_Model;
using System.Globalization;
using System.Net.Http.Json;
using Service_FTP.Data.Calculs;


namespace Service_FTP.Data
{
	public class CsvToMesure
	{
		public static async Task<List<Mesure>> CsvConvertToMesureLimni(string filePath)
		{
			List<Mesure> mesures = new();
			List<Barrage>? barrages = new();
			List<Mesure> mesuresFilter = new();

			HttpClient client = new();
			var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Infos.barrageAPI + "/Barrage/"));
			var response = await client.SendAsync(request).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				barrages = await response.Content.ReadFromJsonAsync<List<Barrage>>().ConfigureAwait(false);
			}

			foreach (string line in File.ReadAllLines(filePath).Skip(2))
			{
				string[] values = line.Split(';');
				Mesure mesure = new();

				mesure.Date = DateTime.Parse(values[0], new CultureInfo("fr-FR")).ToLocalTime();
				mesure.IdBarrage = ConvertBarrageFTPToDB(Convert.ToString(values[1]), barrages!);
				mesure.IdCapteur = ConvertCapteurFTPToDB(Convert.ToString(values[2]), barrages!.First(barrage => barrage.Id == mesure.IdBarrage).Capteurs);
				mesure.Valeur = Convert.ToDecimal(values[3], CultureInfo.InvariantCulture);

				mesuresFilter = mesures.FindAll(x => x.IdBarrage == mesure.IdBarrage);
				mesuresFilter = mesuresFilter.FindAll(x => x.IdCapteur == mesure.IdCapteur);

				if (mesure.IdBarrage == 4)
				{
					mesure.Volume = CalculsSainteCecile.LoiHVSainteCecile(decimal.ToDouble(mesure.Valeur));
					mesure.DebitSortant = CalculsSainteCecile.LoiHQSainteCecile(decimal.ToDouble(mesure.Valeur));
					
					if (mesuresFilter.Count <= 1)
					{
						mesure.DebitEntrant = 0;
						mesure.DebitEntrant15mn = Convert.ToDecimal(2.34);
					}

					else if (mesuresFilter.Count <= 2)
					{
						mesure.DebitEntrant = mesure.DebitSortant + (mesure.Volume - mesuresFilter[^1].Volume) * Convert.ToDecimal(Math.Pow(10, 6)) / (5 * 60);
						mesure.DebitEntrant15mn = Convert.ToDecimal(2.34);
					}

					else
					{
						mesure.DebitEntrant = mesure.DebitSortant + (mesure.Volume - mesuresFilter[^1].Volume) * Convert.ToDecimal(Math.Pow(10, 6)) / (5 * 60);
						mesure.DebitEntrant15mn = (mesure.DebitEntrant + mesuresFilter[^1].DebitEntrant + mesuresFilter[^2].DebitEntrant) / 3;
					}
				}
				else
				{
					mesure.DebitSortant = 0;
					mesure.DebitEntrant15mn = 0;
					mesure.Volume = 0;
				}
				mesures.Add(mesure);
			}
			return mesures;
		}
		public static async Task<List<Mesure>> CsvConvertToMesurePluvio(string filePath)
		{
			List<Mesure> mesures = new();
			List<Barrage>? barrages = new();

			HttpClient client = new();
			var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Infos.barrageAPI + "/Barrage/"));
			var response = await client.SendAsync(request).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				barrages = await response.Content.ReadFromJsonAsync<List<Barrage>>().ConfigureAwait(false);
			}

			foreach (string line in File.ReadAllLines(filePath).Skip(2))
			{
				string[] values = line.Split(';');
				Mesure mesure = new();

				mesure.Date = DateTime.Parse(values[0], new CultureInfo("fr-FR")).ToLocalTime();
				mesure.IdBarrage = ConvertBarrageFTPToDB(Convert.ToString(values[1]), barrages!);
				mesure.IdCapteur = ConvertCapteurFTPToDB(Convert.ToString(values[2]), barrages!.First(barrage => barrage.Id == mesure.IdBarrage).Capteurs);
				mesure.Valeur = Convert.ToDecimal(values[3], CultureInfo.InvariantCulture);
				mesures.Add(mesure);
			}
			return mesures;
		}
		public static int ConvertBarrageFTPToDB(string barrage, List<Barrage> barrages)
		{
			int ret = 0;
			foreach (Barrage barrageDB in barrages)
			{
				if (barrageDB.Libellé == barrage)
				{
					ret = barrageDB.Id;
				}
			}
			return ret;
		}
		public static int ConvertCapteurFTPToDB(string capteur, List<Capteur> capteurs)
		{
			int ret = 0;
			foreach (Capteur capteurDB in capteurs)
			{
				if (capteurDB.Libellé!.Contains(capteur))
				{
					ret = capteurDB.Id;
				}
			}
			return ret;
		}
	}
}
