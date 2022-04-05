using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_FTP.Data.Calculs
{
	public static class CalculsSainteCecile
	{
		public static readonly List<double> LoiHVCotesSainteCecile = new()
		{
			225.00,
			235.00,
			236.00,
			237.00,
			238.00,
			239.00,
			240.00,
			241.00,
			242.00,
			243.00,
			244.00,
			245.00,
			246.00,
			247.00,
			248.00,
			249.00,
			250.00,
			251.00,
			252.00,
			253.00,
			254.00,
			255.00,
			256.00,
			257.00,
			258.00,
			259.00,
			260.00,
			261.00,
			261.34,
			262.00,
			263.00,
			264.00,
			265.00,
			266.00,
			266.80,
			267.00,
			267.70,
			267.75,
			268.00,
			268.10,
			269.00,
			270.00
		};
		public static readonly List<double> LoiHVVolumesSainteCecile = new()
		{
			0.000,
			0.058,
			0.108,
			0.178,
			0.273,
			0.395,
			0.539,
			0.700,
			0.880,
			1.096,
			1.344,
			1.620,
			1.927,
			2.260,
			2.625,
			3.020,
			3.442,
			3.895,
			4.378,
			4.892,
			5.425,
			6.012,
			6.748,
			7.383,
			7.895,
			8.573,
			9.278,
			10.010,
			10.267,
			10.767,
			11.549,
			12.358,
			13.194,
			14.055,
			14.762,
			14.883,
			15.544,
			15.591,
			15.830,
			15.928,
			16.806,
			17.810
		};
		public static readonly List<double> LoiHQCotesSainteCecile = new()
		{
			242.00,
			243.00,
			244.00,
			245.00,
			248.00,
			250.00,
			255.00,
			261.34,
			263.00,
			264.00,
			264.30,
			265.00,
			266.00,
			266.80,
			267.70,
			268.10,
			268.50,
			269.00,
			270.00
		};
		public static readonly List<double> LoiHQDebitSainteCecile = new() { 0, 23, 64, 117, 188, 209, 254, 302, 527, 754, 831, 880, 900, 920, 925, 939, 989, 1137, 1581 };

		public static decimal LoiHVSainteCecile(double valeur)
		{
			double volume = 0;

			for (int i = 0; i < 41; i++)
			{
				if (valeur >= LoiHVCotesSainteCecile[i] && valeur < LoiHVCotesSainteCecile[i + 1])
				{
					volume = LoiHVVolumesSainteCecile[i] + (LoiHVVolumesSainteCecile[i + 1] - LoiHVVolumesSainteCecile[i]) / (LoiHVCotesSainteCecile[i + 1] - LoiHVCotesSainteCecile[i]) * (valeur - LoiHVCotesSainteCecile[i]);
				}
			}

			return Convert.ToDecimal(volume);
		}
		public static decimal LoiHQSainteCecile(double valeur)
		{
			double debitSortant = 0.2;

			for (int i = 0; i < 18; i++)
			{
				if (valeur >= LoiHQCotesSainteCecile[i] && valeur < LoiHQCotesSainteCecile[i + 1])
				{
					debitSortant = LoiHQDebitSainteCecile[i] + (LoiHQDebitSainteCecile[i + 1] - LoiHQDebitSainteCecile[i]) / (LoiHQCotesSainteCecile[i + 1] - LoiHQCotesSainteCecile[i]) * (valeur - LoiHQCotesSainteCecile[i]);
				}
			}

			return Convert.ToDecimal(debitSortant);
		}
	}
}
