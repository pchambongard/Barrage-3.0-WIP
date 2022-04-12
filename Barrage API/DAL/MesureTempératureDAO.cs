using System.Globalization;
using Barrage_Model;
using Npgsql;

namespace Barrage_API.DAL
{
	public class MesureTempératureDAO
	{
		public static async Task<List<MesureTempérature>> GetMesureBetweenByCapteur(DateTime date, DateTime date2, int id_capteur)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);
				await connection.OpenAsync().ConfigureAwait(false);

				List<MesureTempérature> mesures = new();

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id_capteur, gdh, valeur FROM mesures_temperature m WHERE id_capteur=:id_capteur AND m.gdh >= :date AND m.gdh <= :date2;"
				};

				commande.Parameters.Add(new NpgsqlParameter<DateTime>(":date", date));
				commande.Parameters.Add(new NpgsqlParameter<DateTime>(":date2", date2));
				commande.Parameters.Add(new NpgsqlParameter<int>(":id_capteur", id_capteur));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					while (await reader.ReadAsync())
					{
						MesureTempérature mesure = new(
							idCapteur: reader.GetInt32(reader.GetOrdinal("id_capteur")),
							date: reader.GetDateTime(reader.GetOrdinal("gdh")),
							valeur: reader.GetDecimal(reader.GetOrdinal("valeur")));
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
		public static async Task<MesureTempérature> GetLastMesureByCapteur(int id_capteur)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);
				await connection.OpenAsync().ConfigureAwait(false);

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id_capteur, gdh, valeur FROM mesures_temperature m WHERE id_capteur=:id_capteur AND m.gdh = (SELECT MAX(gdh) FROM mesures_temperature WHERE id_capteur=:id_capteur)"
				};
				commande.Parameters.Add(new NpgsqlParameter<int>(":id_capteur", id_capteur));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					reader.Read();

					MesureTempérature mesure = new(
							idCapteur: id_capteur,
							date: reader.GetDateTime(reader.GetOrdinal("gdh")),
							valeur: reader.GetDecimal(reader.GetOrdinal("valeur")));

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
		public static async Task<int> StoreAsync(List<MesureTempérature> mesures)
		{
			try
			{
				await using NpgsqlConnection connection = new(Infos.dbConn);

				await connection.OpenAsync().ConfigureAwait(false);

				string commande = "INSERT INTO mesures_temperature(id_capteur, gdh, valeur) values";

				foreach (MesureTempérature Mesure in mesures)
				{
					string str = $"({Mesure.IdCapteur},'{Mesure.Date.ToString("yyyy-MM-dd HH:mm:ss")}',{decimal.Round(Mesure.Valeur, 3).ToString(CultureInfo.InvariantCulture)}),";
					commande += str;
				}
				char[] tmp = commande.ToCharArray();
				tmp[^1] = ' ';
				commande = new(tmp);
				commande += "ON CONFLICT ON CONSTRAINT \"mesures_temperature_id_capteur_gdh_key\" DO NOTHING;"; //  Si la mesure existe deja , on ne leve pas d'exception mais on passe a la suivante

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
