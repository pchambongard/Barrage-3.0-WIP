using Model;

namespace WebSite.Data
{
	public class ChartItem
	{
		public List<DateTime> Dates { get; set; } = new();
		public List<Capteur> LimniCapteurs { get; set; } = new();
		public List<Capteur> PluvioCapteurs { get; set; } = new();
		public List<Capteur> TempCapteurs { get; set; } = new();
		public List<List<Item>> CoteItem { get; set; } = new();
		public List<List<Item>> LimniItem { get; set; } = new();
		public List<List<Item>> PluvioItem { get; set; } = new();
		public List<List<Item>> TempItem { get; set; } = new();

		public void RenewChart()
		{
			LimniCapteurs = new();
			PluvioCapteurs = new();
			TempCapteurs = new();
			CoteItem = new();
			LimniItem = new();
			PluvioItem = new();
			TempItem = new();
		}
		public void SetCapteurInChart(Capteur capteur, List<Mesure>? mesures)
		{
			if (capteur.Type!.Id == 1)
			{
				LimniCapteurs.Add(capteur);
				LimniItem.Add(new());
				foreach (var mesure in mesures!)
				{
					LimniItem[^1].Add(new Item(mesure.Gdh, decimal.Round(mesure.Valeur, 3)));
				}
				LimniItem[^1].Sort((x, y) => x.Date.CompareTo(y.Date));
			}
			else if (capteur.Type!.Id == 2)
			{
				PluvioCapteurs.Add(capteur);
				PluvioItem.Add(new());
				foreach (var mesure in mesures!)
				{
					PluvioItem[^1].Add(new Item(mesure.Gdh, decimal.Round(mesure.Valeur, 3)));
				}
				PluvioItem[^1].Sort((x, y) => x.Date.CompareTo(y.Date));
			}
			else if (capteur.Type!.Id == 3)
			{
				TempCapteurs.Add(capteur);
				TempItem.Add(new());
				foreach (var mesure in mesures!)
				{
					TempItem[^1].Add(new Item(mesure.Gdh, decimal.Round(mesure.Valeur, 3)));
				}
				TempItem[^1].Sort((x, y) => x.Date.CompareTo(y.Date));
			}
		}
		public void SetCote(Barrage? barrage)
		{

			for (int i = 0; i < barrage!.CotesExploitation.Count; i++)
			{
				CoteItem.Add(new());
				CoteItem[i].Add(new Item(Dates[0], barrage!.CotesExploitation[i].Seuil));
				CoteItem[i].Add(new Item(Dates[1], barrage!.CotesExploitation[i].Seuil));
			}
		}
		public void SetDate(DateTime date1, DateTime date2)
		{
			Dates = new();
			Dates.Add(date1);
			Dates.Add(date2);
		}
	}
	public class Item
	{
		public DateTime Date { get; set; }
		public decimal Valeur { get; set; }

		public Item(DateTime date, decimal valeur)
		{
			Date = date;
			Valeur = valeur;
		}
	}
}
