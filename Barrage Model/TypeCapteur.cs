using Barrage_Model.Utils;

namespace Barrage_Model
{
	public class TypeCapteur : LabelledIdBase
	{
		#region constructeurs
		public TypeCapteur() : base()
		{
			// Empty for deserializer
		}
		public TypeCapteur(string libellé) : base(libellé)
		{
			// empty on purpose
		}
		public TypeCapteur(int id, string libellé) : base(id, libellé)
		{
			// empty on purpose
		}
		#endregion constructeurs
	}
}
