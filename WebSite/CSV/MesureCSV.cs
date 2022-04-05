using System.Text;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace WebSite.CSV
{
	public class MesureLimniCSV
	{
		public string Barrage { get; set; }
		public string Capteur { get; set; }
		public DateTime Date { get; set; }
		public decimal Valeur { get; set; }
		public decimal Volume { get; set; }
		public decimal DebitSortant { get; set; }
		public decimal DebitEntrant15mn { get; set; }

		public MesureLimniCSV(string barrage, string capteur, DateTime date, decimal valeur)
		{

			Barrage = barrage;
			Capteur = capteur;
			Date = date;
			Valeur = valeur;
		}
		public MesureLimniCSV(string barrage, string capteur, DateTime date, decimal valeur, decimal volume, decimal debitSortant, decimal debitEntrant15mn)
		{

			Barrage = barrage;
			Capteur = capteur;
			Date = date;
			Valeur = valeur;
			Volume = volume;
			DebitSortant = debitSortant;
			DebitEntrant15mn = debitEntrant15mn;

		}
		public static void MesuresToCSV(List<MesureLimniCSV> mesures, string barrage, string type)
		{
			mesures.Sort((x, y) => y.Date.CompareTo(x.Date));
			barrage = barrage.Replace(" ", string.Empty);
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + type + barrage + DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy-HH-mm") + ".csv";

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				Encoding = Encoding.UTF8,
			};
			using var writer = new StreamWriter(path);
			using var csv = new CsvWriter(writer, config);
			csv.WriteRecords(mesures);
		}
	}
	public class MesurePluvioCSV
	{
		public string Barrage { get; set; }
		public string Capteur { get; set; }
		public DateTime Date { get; set; }
		public decimal Valeur { get; set; }

		public MesurePluvioCSV(string barrage, string capteur, DateTime date, decimal valeur)
		{

			Barrage = barrage;
			Capteur = capteur;
			Date = date;
			Valeur = valeur;
		}
		public static void MesuresToCSV(List<MesurePluvioCSV> mesures, string barrage, string type)
		{
			mesures.Sort((x, y) => y.Date.CompareTo(x.Date));
			barrage = barrage.Replace(" ", string.Empty);
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + type + barrage + DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy-HH-mm") + ".csv";

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				Encoding = Encoding.UTF8,
			};
			using var writer = new StreamWriter(path);
			using var csv = new CsvWriter(writer, config);
			csv.WriteRecords(mesures);
		}
	}
	public class MesureTempCSV
	{
		public string Barrage { get; set; }
		public string Capteur { get; set; }
		public string Date { get; set; }
		public decimal ValeurMin { get; set; }
		public decimal ValeurMax { get; set; }

		public MesureTempCSV(string barrage, string capteur, string date, decimal valeurMin, decimal valeurMax)
		{
			Barrage = barrage;
			Capteur = capteur;
			Date = date;
			ValeurMin = valeurMin;
			ValeurMax = valeurMax;
		}
		public static void MesuresToCSV(List<MesureTempCSV> mesures, string barrage, string type)
		{
			mesures.Sort((x, y) => y.Date.CompareTo(x.Date));
			barrage = barrage.Replace(" ", string.Empty);
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + type + barrage + DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy-HH-mm") + ".csv";

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				Encoding = Encoding.UTF8,
			};
			using var writer = new StreamWriter(path);
			using var csv = new CsvWriter(writer, config);
			csv.WriteRecords(mesures);
		}
	}
	public class MesurePluvioQuotidienneCSV
	{
		public string Barrage { get; set; }
		public string Capteur { get; set; }
		public DateTime Date { get; set; }
		public decimal Valeur { get; set; }

		public MesurePluvioQuotidienneCSV(string barrage, string capteur, DateTime date, decimal valeur)
		{

			Barrage = barrage;
			Capteur = capteur;
			Date = date;
			Valeur = valeur;
		}
		public static void MesuresToCSV(List<MesurePluvioQuotidienneCSV> mesures, string barrage, string type)
		{
			mesures.Sort((x, y) => y.Date.CompareTo(x.Date));
			barrage = barrage.Replace(" ", string.Empty);
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + type + barrage + DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy-HH-mm") + ".csv";

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				Encoding = Encoding.UTF8,
			};
			using var writer = new StreamWriter(path);
			using var csv = new CsvWriter(writer, config);
			csv.WriteRecords(mesures);
		}
	}
}
