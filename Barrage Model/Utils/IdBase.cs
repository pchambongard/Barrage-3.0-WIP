namespace Barrage_Model.Utils
{
	public abstract class IdBase
	{
		#region propriétés publiques
		public int Id { get; set; }
		#endregion propriétés publiques

		#region constructeurs
		public IdBase()
		{
			// empty on purpose
		}
		public IdBase(int id)
		{
			Id = id;
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
