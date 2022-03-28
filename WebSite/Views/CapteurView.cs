namespace WebSite.Views
{
    public class CapteurView
    {
        public int IdCapteur { get; set; } = 0;
        public string Barrage { get; set; } = string.Empty;
        public string Capteur { get; set; } = string.Empty;
        public string Date { get; set; } = "";
        public string Cote { get; set; } = "";
        public decimal Valeur { get; set; }

        public CapteurView(int id, string barrage, string capteur)
        {
            IdCapteur = id;
            Barrage = barrage;
            Capteur = capteur;
        }
    }
}
