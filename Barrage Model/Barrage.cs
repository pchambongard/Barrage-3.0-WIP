using Barrage_Model.Utils;

namespace Barrage_Model
{
	public class Barrage : LabelledIdBase
	{
		#region propriétés publiques
		public TypeBarrage? Type { get; set; }
		public List<Capteur> Capteurs { get; set; } = new();
		public List<CoteExploitation> CotesExploitation { get; set; } = new();
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public decimal Altitude { get; set; }
		public int Hauteur { get; set; }
		public int Longueur { get; set; }
		public int VolumeRetenu { get; set; }
		public int SurfaceRetenue { get; set; }
		public int SurfaceBassin { get; set; }
		public string? Usage { get; set; }
		#endregion propriétés publiques

		#region constructeurs
		public Barrage() : base()
		{
			// Empty for deserializer
		}
		public Barrage(string libellé, TypeBarrage type) : base(libellé)
		{
			Type = type;
		}
		public Barrage(int id, string libellé, TypeBarrage type) : base(id, libellé)
		{
			Type = type;
		}
		#endregion constructeurs

		#region methods 
		public bool AddCapteur(Capteur newCapteur)
		{
			foreach (Capteur capteur in Capteurs)
			{
				if (capteur.Libellé == newCapteur.Libellé)
				{
					return false;
				}
				if (capteur.Principal == true && newCapteur.Principal == true && newCapteur.Type == capteur.Type)
				{
					return false;
				}
			}
			Capteurs.Add(newCapteur);
			return true;
		}
		public void AddCote(CoteExploitation newCote)
		{
			foreach (CoteExploitation cote in CotesExploitation)
			{
				if (cote.Id == newCote.Id)
				{
					throw new ArgumentException(nameof(Barrage) + " " + nameof(AddCote) + ": Id déjà présent.");
				}
				if (cote.Type == newCote.Type && cote.Seuil == newCote.Seuil)
				{
					throw new ArgumentException(nameof(Barrage) + " " + nameof(AddCote) + ": Il existe déjà une cote avec le meme seuil");
				}
				if (cote.Type == newCote.Type &&
					(cote.Seuil > newCote.Seuil && cote.Criticité <= newCote.Criticité ||
					cote.Seuil < newCote.Seuil && cote.Criticité >= newCote.Criticité))
				{
					throw new ArgumentException(nameof(Barrage) + " " + nameof(AddCote) + ": Cote invalide. Verifiez les valeurs de cette cote par rapport aux autres.");
				}
			}
			CotesExploitation.Add(newCote);
		}
		#endregion methods
	}
}