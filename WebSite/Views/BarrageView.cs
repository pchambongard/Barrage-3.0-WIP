using Barrage_Model;
namespace WebSite.Views
{
	public class BarrageView
	{
		public int Id { get; set; }
		public String Libellé { get; set; }
		public CapteurView CapteurPrincipal { get; set; } = new();
		public List<CoteExploitation> Cotes { get; set; } = new();
		public List<CapteurView> Capteurs { get; set; } = new();
		public double Crue { get; set; } = 0;
		public double Danger { get; set; } = 0;
		public double Min { get; set; } = 0;
		public double Max { get; set; } = 0;
		public double Step { get; set; } = 0;

		public BarrageView(int id, string libellé)
		{
			Id = id;
			Libellé = libellé;
		}
		public void SetGauge()
		{

			if (CapteurPrincipal.Capteur == String.Empty && Capteurs.Find(x => x.Type == "Limnimétrie") != null)
			{
				CapteurPrincipal = Capteurs.Find(x => x.Type == "Limnimétrie")!;
			}
			Capteurs.Sort((x, y) => { int ret = String.Compare(x.Capteur, y.Capteur); return ret; });
			if (Cotes.Find(x => x.Criticité == 2) != null)
			{
				Crue = Math.Truncate((double)Cotes.Find(x => x.Criticité == 2)!.Seuil);
				Min = Math.Truncate((double)Cotes.Find(x => x.Criticité == 0)!.Seuil * 0.925);
				if (Cotes.Find(x => x.Criticité == 4) != null)
				{
					Danger = (double)Cotes.Find(x => x.Criticité == 4)!.Seuil;
					Max = Math.Truncate(Danger * 1.05);
				}
				else
				{
					Max = Math.Truncate(Crue * 1.05);
					Danger = Max;
				}
				SetMinMax();
				Step = (Max - Min) / 4;
			}
		}
		public void SetMinMax()
		{
			switch (Id)
			{
				case 1:
					{
						Min = 145;
						Max = 160;
						break;
					}
				case 2:
					{
						Min = 105;
						Max = 130;
						break;
					}
				case 3:
					{
						Min = 70;
						Max = 90;
						break;
					}
				case 4:
					{
						Min = 230;
						Max = 267;
						break;
					}
				case 5:
					{
						Min = 210;
						Max = 272;
						break;
					}
				default:
					break;

			}
		}
	}
}
