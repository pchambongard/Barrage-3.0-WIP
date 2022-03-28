using Barrage_Model;
using Npgsql;

namespace Barrage_API.DAL
{
	public class CapteurDAO
	{
		public static async Task<Capteur> GetByIdAsync(int id)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id, nom, id_type_capteur, est_principal, est_actif FROM capteurs c WHERE c.id = :id;"
				};

				commande.Parameters.Add(new NpgsqlParameter<int>(":id", id));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					await reader.ReadAsync();

					Capteur capteur = new(
						id: reader.GetInt32(reader.GetOrdinal("id")),
						libellé: reader.GetString(reader.GetOrdinal("nom")),
						type: await TypeCapteurDAO.GetByIdAsync(reader.GetInt32(reader.GetOrdinal("id_type_capteur"))),
						principal: reader.GetBoolean(reader.GetOrdinal("est_principal")),
						actif: reader.GetBoolean(reader.GetOrdinal("est_actif")));

					connection.Close();
					connection.Dispose();

					return capteur;
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

		public static async Task<List<Capteur>> GetByBarrageAsync(int idbarrage)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				List<Capteur> capteurs = new();

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id, nom, id_type_capteur, est_principal, est_actif FROM capteurs c WHERE c.id_barrage = :idbarrage;"
				};

				commande.Parameters.Add(new NpgsqlParameter<int>(":idbarrage", idbarrage));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					while (await reader.ReadAsync())
					{
						Capteur capteur = new(
							id: reader.GetInt32(reader.GetOrdinal("id")),
							libellé: reader.GetString(reader.GetOrdinal("nom")),
							type: await TypeCapteurDAO.GetByIdAsync(reader.GetInt32(reader.GetOrdinal("id_type_capteur"))),
							principal: reader.GetBoolean(reader.GetOrdinal("est_principal")),
							actif: reader.GetBoolean(reader.GetOrdinal("est_actif")));

						capteurs.Add(capteur);
					}

					connection.Close();
					connection.Dispose();

					return capteurs;
				}
				else
				{
					return new List<Capteur>();
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}

		public static async Task<List<Capteur>> GetAllAsync()
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				List<Capteur> capteurs = new();

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id, nom, id_type_capteur, est_principal, est_actif FROM capteurs"
				};

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					while (await reader.ReadAsync())
					{
						Capteur capteur = new(
							id: reader.GetInt32(reader.GetOrdinal("id")),
							libellé: reader.GetString(reader.GetOrdinal("nom")),
							type: await TypeCapteurDAO.GetByIdAsync(reader.GetInt32(reader.GetOrdinal("id_type_capteur"))),
							principal: reader.GetBoolean(reader.GetOrdinal("est_principal")),
							actif: reader.GetBoolean(reader.GetOrdinal("est_actif")));

						capteurs.Add(capteur);
					}

					connection.Close();
					connection.Dispose();

					return capteurs;
				}
				else
				{
					return new List<Capteur>();
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}

		public static async Task<int> StoreAsync(Capteur capteur, int idBarrage)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				if (capteur.IsNew())
				{
					await using NpgsqlCommand commande = new()
					{
						Connection = connection,
						CommandText = "INSERT INTO capteurs(nom, id_type_capteur, id_barrage, est_principal, est_actif) values(:nom, :type, :barrage, :principal, :actif) returning id;"
					};

					commande.Parameters.Add(new NpgsqlParameter<string>(":nom", capteur.Libellé ?? ""));
					commande.Parameters.Add(new NpgsqlParameter<int>(":type", capteur.Type?.Id ?? 0));
					commande.Parameters.Add(new NpgsqlParameter<int>(":barrage", idBarrage));
					commande.Parameters.Add(new NpgsqlParameter<bool>(":principal", capteur.Principal));
					commande.Parameters.Add(new NpgsqlParameter<bool>(":actif", capteur.Actif));

					capteur.Id = (short)await commande.ExecuteNonQueryAsync();

					connection.Close();
					connection.Dispose();

					return 0;
				}
				else
				{
					await using NpgsqlCommand commande = new()
					{
						Connection = connection,
						CommandText = "UPDATE capteurs SET nom=:nom, id_type_capteur=:type, id_barrage=:id_barrage, est_principal=:principal, est_actif=:actif WHERE id=" + capteur.Id
					};

					commande.Parameters.Add(new NpgsqlParameter<string>(":nom", capteur.Libellé ?? ""));
					commande.Parameters.Add(new NpgsqlParameter<int>(":type", capteur.Type?.Id ?? 0));
					commande.Parameters.Add(new NpgsqlParameter<int>(":id_barrage", idBarrage));
					commande.Parameters.Add(new NpgsqlParameter<bool>(":principal", capteur.Principal));
					commande.Parameters.Add(new NpgsqlParameter<bool>(":actif", capteur.Actif));

					await commande.ExecuteNonQueryAsync();

					connection.Close();
					connection.Dispose();

					return 1;
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}
	}
}
