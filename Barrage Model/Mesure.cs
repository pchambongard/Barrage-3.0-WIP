namespace Barrage_Model
{
	public class Mesure
	{
		#region propriétés publiques
		public int IdBarrage { get; set; }
		public int IdCapteur { get; set; }
		public DateTime Date { get; set; }
		public decimal Valeur { get; set; }
		public decimal Volume { get; set; }
		public decimal DebitSortant { get; set; }
		public decimal DebitEntrant { get; set; }
		public decimal DebitEntrant15mn { get; set; }
		#endregion propriétés publiques

		#region constructeurs

		public Mesure()
		{
			// Empty for deserializer
		}
		public Mesure(int idBarrage, int idCapteur, DateTime date, decimal valeur)
		{
			IdBarrage = idBarrage;
			IdCapteur = idCapteur;
			Date = date;
			Valeur = valeur;
		}
		public Mesure(DateTime date, decimal valeur)
		{
			Date = date;
			Valeur = valeur;
		}
		public Mesure(DateTime date, decimal valeur, int idBarrage, int idCapteur, decimal debitSortant, decimal debitEntrant15mn, decimal volume)
		{
			IdBarrage = idBarrage;
			IdCapteur = idCapteur;
			Date = date;
			Valeur = valeur;
			Volume = volume;
			DebitSortant = debitSortant;
			DebitEntrant15mn = debitEntrant15mn;
		}
		#endregion constructeurs
	}
}
