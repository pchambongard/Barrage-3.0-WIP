using Barrage_Model;
using Npgsql;

namespace Barrage_API.DAL
{
	public class MesurePluvioQuotidienneDAO
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
					CommandText = "SELECT id_capteur, gdh, valeur FROM mesures_jour m WHERE id_capteur=:id_capteur AND m.gdh >= :date AND m.gdh <= :date2;"
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
	}
}
