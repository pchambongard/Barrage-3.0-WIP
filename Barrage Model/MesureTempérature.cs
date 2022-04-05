using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barrage_Model
{
	public class MesureTempérature
	{
		public int IdCapteur { get; set; }
		public DateTime Date { get; set; }
		public decimal Valeur { get; set; }

		public MesureTempérature()
		{
			// Empty for deserializer
		}
		public MesureTempérature(int idCapteur, DateTime date, decimal valeur)
		{
			IdCapteur = idCapteur;
			Date = date;
			Valeur = valeur;
		}

	}
}
