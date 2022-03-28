namespace Barrage_Model
{
	public class Mesure
	{
		#region propriétés publiques
		public int IdBarrage { get; set; }
		public int IdCapteur { get; set; }
		public DateTime Gdh { get; set; }
		public decimal Valeur { get; set; }
		public decimal DebitSortant { get; set; }
		public decimal DebitEntrant { get; set; }
		public decimal Volume { get; set; }
		public decimal ValeurCote { get; set; }
		public decimal VolumeRetenu { get; set; }
		#endregion propriétés publiques

		#region constructeurs

		public Mesure()
		{
			// Empty for deserializer
		}
		public Mesure(DateTime date, decimal valeur, int idBarrage, int idCapteur)
		{
			Gdh = date;
			Valeur = valeur;
			IdBarrage = idBarrage;
			IdCapteur = idCapteur;
		}
		#endregion constructeurs
	}
}
