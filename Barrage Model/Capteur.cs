using Barrage_Model.Utils;

namespace Barrage_Model
{
	public class Capteur : LabelledIdBase
	{
		#region propriétés publiques
		public TypeCapteur? Type { get; set; }
		public bool Principal { get; set; } = false;
		public bool Actif { get; set; } = true;
		#endregion propriétés publiques

		#region constructeurs
		public Capteur() : base()
		{
			// Empty for deserializer
		}
		public Capteur(string libellé, TypeCapteur type, bool principal, bool actif) : base(libellé)
		{
			Principal = principal;
			Actif = actif;
			Type = type;
		}
		public Capteur(int id, string libellé, TypeCapteur type, bool principal, bool actif) : base(id, libellé)
		{
			Principal = principal;
			Actif = actif;
			Type = type;
		}
		#endregion constructeurs
	}
}
