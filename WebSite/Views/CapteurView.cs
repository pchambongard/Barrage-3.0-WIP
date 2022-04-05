namespace WebSite.Views
{
    public class CapteurView
	{
		public string Capteur { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;
		public string Date { get; set; } = string.Empty;
		public string DerniereCoteAtteinte { get; set; } = string.Empty;
		public double Valeur { get; set; } = 0;

		public CapteurView()
		{
			//Empty on purpose
		}
		public CapteurView(string capteur, string type, DateTime date, double valeur)
		{
			Capteur = capteur;
			Type = type;
			Date = date.ToString();
			Valeur = valeur;
		}
	}
}
