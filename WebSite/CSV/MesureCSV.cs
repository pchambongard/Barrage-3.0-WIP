using System.Text;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace WebSite.CSV
{
    public class MesureCSV
    {
        public int IdCapteur { get; set; }
        public int IdBarrage { get; set; }
        public DateTime Gdh { get; set; }
        public decimal Valeur { get; set; }
        public decimal DebitSortant { get; set; }
        public decimal DebitEntrant { get; set; }
        public decimal Volume { get; set; }
        public decimal ValeurCote { get; set; }
        public decimal VolumeRetenu { get; set; }
    
        public MesureCSV(int idCapteur, int idBarrage, DateTime gdh, decimal valeur)
        {

            IdCapteur = idCapteur;
            IdBarrage = idBarrage;
            Gdh = gdh;
            Valeur = valeur;
		}
		public static void MesuresToCSV(List<MesureCSV> mesures, string barrage, string type)
		{
			string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Mesures" + type + barrage + ".csv";
			mesures.Sort((x, y) => y.Gdh.CompareTo(x.Gdh));

			if (File.Exists(Path))
			{
				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					Delimiter = ";",
					Encoding = Encoding.UTF8,
					HasHeaderRecord = false,
				};
				using var stream = File.Open(Path, FileMode.Append);
				using var writer = new StreamWriter(stream);
				using var csv = new CsvWriter(writer, config);
				csv.WriteRecords(mesures);
			}
			else
			{
				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					Delimiter = ";",
					Encoding = Encoding.UTF8,
				};
				using var writer = new StreamWriter(Path);
				using var csv = new CsvWriter(writer, config);
				csv.WriteRecords(mesures);
			}
		}
	}
}
