using Barrage_Model;

namespace WebSite.Views
{
	public class ChartView
	{
		public bool IsLoaded { get; set; } = false;
		public Barrage Barrage { get; set; } = new();
		public List<DateTime> StartEnd { get; set; } = new();
		public TimeSpan Step { get; set; } = new();
		public List<LimniItem> LimniItems { get; set; } = new();
		public List<PluvioItem> PluvioItems { get; set; } = new();
		public List<TempItem> TempItems { get; set; } = new();
		public void SetDate(DateTime date1, DateTime date2)
		{
			StartEnd = new();
			StartEnd.Add(date1);
			StartEnd.Add(date2);

			Step = (date2 - date1) / 5;
		}
	}

	public class LimniItem
	{
		public Capteur Capteur { get; set; } = new();
		public List<Mesure> Mesures { get; set; } = new();

		public LimniItem(Capteur capteur, List<Mesure> mesures)
		{
			Capteur = capteur;
			Mesures = mesures;
		}
	}
	public class PluvioItem
	{
		public Capteur Capteur { get; set; } = new();
		public List<Mesure> Mesures { get; set; } = new();
		public PluvioItem(Capteur capteur, List<Mesure> mesures)
		{
			Capteur = capteur;
			Mesures = mesures;
		}

	}
	public class TempItem
	{
		public Capteur Capteur { get; set; } = new();
		public List<MesureTempérature> Mesures { get; set; } = new();
		public TempItem(Capteur capteur, List<MesureTempérature> mesures)
		{
			Capteur = capteur;
			Mesures = mesures;
		}
	}
}
