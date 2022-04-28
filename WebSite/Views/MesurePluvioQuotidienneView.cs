namespace WebSite.Views
{
	public class MesurePluvioQuotidienneView
	{
		public string Date { get; set; }
		public decimal Valeur { get; set; }


		public MesurePluvioQuotidienneView(string date, decimal valeur)
		{
			Date = date;
			Valeur = valeur;
		}
	}
}
