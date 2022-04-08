using System.Globalization;
using Npgsql;
using Barrage_Model;

namespace Barrage_API.DAL
{
	public class MesureDAO
	{
		public static async Task<List<Mesure>> GetMesureBetweenByBarrage(DateTime date, DateTime date2, int id_barrage)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);
				await connection.OpenAsync().ConfigureAwait(false);

				List<Mesure> mesures = new();

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id_capteur, id_barrage, gdh, valeur, debit_sortant, debit_entrant, volume  FROM mesures m WHERE id_barrage=:id_barrage AND m.gdh >= :date AND m.gdh <= :date2;"
				};

				commande.Parameters.Add(new NpgsqlParameter<DateTime>(":date", date));
				commande.Parameters.Add(new NpgsqlParameter<DateTime>(":date2", date2));
				commande.Parameters.Add(new NpgsqlParameter<int>(":id_barrage", id_barrage));
				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					while (await reader.ReadAsync())
					{
						Mesure mesure = new(
							idCapteur: reader.GetInt32(reader.GetOrdinal("id_capteur")),
							idBarrage: id_barrage,
							date: reader.GetDateTime(reader.GetOrdinal("gdh")),
							valeur: reader.GetDecimal(reader.GetOrdinal("valeur")),
							debitSortant: reader.GetDecimal(reader.GetOrdinal("debit_sortant")),
							debitEntrant15mn: reader.GetDecimal(reader.GetOrdinal("debit_entrant")),
							volume: reader.GetDecimal(reader.GetOrdinal("volume")));
						mesures.Add(mesure);

					}
					connection.Close();
					connection.Dispose();
					return mesures;
				}
				else
				{
					throw new Exception("Aucun élément correspondant.");
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}
		public static async Task<List<Mesure>> GetMesureBetweenByCapteur(DateTime date, DateTime date2, int id_barrage, int id_capteur)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);
				await connection.OpenAsync().ConfigureAwait(false);

				List<Mesure> mesures = new();

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id_capteur, id_barrage, gdh, valeur, debit_sortant, debit_entrant, volume FROM mesures m WHERE id_capteur=:id_capteur AND m.gdh >= :date AND m.gdh <= :date2;"
				};

				commande.Parameters.Add(new NpgsqlParameter<DateTime>(":date", date));
				commande.Parameters.Add(new NpgsqlParameter<DateTime>(":date2", date2));
				commande.Parameters.Add(new NpgsqlParameter<int>(":id_capteur", id_capteur));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					while (await reader.ReadAsync())
					{
						Mesure mesure = new(
							idCapteur: reader.GetInt32(reader.GetOrdinal("id_capteur")),
							idBarrage: id_barrage,
							date: reader.GetDateTime(reader.GetOrdinal("gdh")),
							valeur: reader.GetDecimal(reader.GetOrdinal("valeur")),
							debitSortant: reader.GetDecimal(reader.GetOrdinal("debit_sortant")),
							debitEntrant15mn: reader.GetDecimal(reader.GetOrdinal("debit_entrant")),
							volume: reader.GetDecimal(reader.GetOrdinal("volume")));
						mesures.Add(mesure);

					}
					connection.Close();
					connection.Dispose();
					return mesures;
				}
				else
				{
					throw new Exception("Aucun élément correspondant.");
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}
		public static async Task<Mesure> GetLastMesureByCapteur(int id_barrage, int id_capteur)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);
				await connection.OpenAsync().ConfigureAwait(false);

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id_capteur, id_barrage, gdh, valeur, debit_sortant, debit_entrant, volume FROM mesures m WHERE id_capteur=:id_capteur AND m.gdh = (SELECT MAX(gdh) FROM mesures WHERE id_capteur=:id_capteur)"
				};
				commande.Parameters.Add(new NpgsqlParameter<int>(":id_capteur", id_capteur));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					reader.Read();

					Mesure mesure = new(
							idCapteur: id_capteur,
							idBarrage: id_barrage,
							date: reader.GetDateTime(reader.GetOrdinal("gdh")),
							valeur: reader.GetDecimal(reader.GetOrdinal("valeur")),
							debitSortant: reader.GetDecimal(reader.GetOrdinal("debit_sortant")),
							debitEntrant15mn: reader.GetDecimal(reader.GetOrdinal("debit_entrant")),
							volume: reader.GetDecimal(reader.GetOrdinal("volume")));

					connection.Close();
					connection.Dispose();
					return mesure;
				}
				else
				{
					throw new Exception("Aucun élément correspondant.");
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}
		public static async Task<int> StoreAsync(List<Mesure> mesures)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);

				await connection.OpenAsync().ConfigureAwait(false);

				string commande = "INSERT INTO mesures(id_barrage, id_capteur, gdh, valeur, debit_sortant, debit_entrant, volume) values";

				foreach (Mesure Mesure in mesures)
				{
					string str = $"({Mesure.IdBarrage},{Mesure.IdCapteur},'{Mesure.Date}'," +
						$"{decimal.Round(Mesure.Valeur, 3).ToString(CultureInfo.InvariantCulture)}, " +
						$"{decimal.Round(Mesure.DebitSortant, 3).ToString(CultureInfo.InvariantCulture)}, " +
						$"{decimal.Round(Mesure.DebitEntrant15mn, 3).ToString(CultureInfo.InvariantCulture)}, " +
						$"{decimal.Round(Mesure.Volume, 3).ToString(CultureInfo.InvariantCulture)}),";
					commande += str;
				}
				char[] tmp = commande.ToCharArray();
				tmp[^1] = ' ';
				commande = new(tmp);
				commande += "ON CONFLICT ON CONSTRAINT \"UK_gdh_capteur\" DO NOTHING;"; //  Si la mesure existe deja , on ne leve pas d'exception mais on passe a la suivante

				await using NpgsqlCommand command = new()
				{
					Connection = connection,
					CommandText = commande
				};

				await command.ExecuteNonQueryAsync();

				connection.Close();
				connection.Dispose();
				return 0;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}
	}
}
