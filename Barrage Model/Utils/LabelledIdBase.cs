namespace Barrage_Model.Utils
{
	public abstract class LabelledIdBase
	{
		#region propriétés publiques
		public int Id { get; set; }
		public string? Libellé { get; set; }
		#endregion propriétés publiques

		#region constructeurs
		public LabelledIdBase()
		{
			// Empty on purpose
		}
		public LabelledIdBase(string libellé)
		{
			Libellé = libellé;
		}
		public LabelledIdBase(int id, string libellé)
		{
			Id = id;
			Libellé = libellé;
		}
		#endregion constructeurs

		#region methodes publiques
		public bool IsNew()
		{
			return Id == 0;
		}
		#endregion methodes publiques
	}
}
