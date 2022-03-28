using Model.Utils;

namespace Model
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
            this.Principal = principal;
            this.Actif = actif;
            this.Type = type;
        }
        public Capteur(int id, string libellé, TypeCapteur type, bool principal, bool actif) : base(id, libellé)
        {
            this.Principal = principal;
            this.Actif = actif;
            this.Type = type;
        }
        #endregion constructeurs
    }
}
