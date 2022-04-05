using Barrage_Model;
using Npgsql;

namespace Barrage_API.DAL
{
	public class BarrageDAO
	{
		public static async Task<Barrage> GetByIdAsync(int id)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id, nom, id_type_barrage, usage, hauteur, longueur, " +
					"volume_retenu, surface_retenue, surface_bassin, latitude, longitude, altitude FROM barrages b WHERE b.id = :id;"
				};

				commande.Parameters.Add(new NpgsqlParameter<int>(":id", id));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					await reader.ReadAsync();

					Barrage barrage = new(
						id: reader.GetInt32(reader.GetOrdinal("id")),
						libellé: reader.GetString(reader.GetOrdinal("nom")),
						type: await TypeBarrageDAO.GetByIdAsync(reader.GetInt32(reader.GetOrdinal("id_type_barrage"))));

					barrage.Usage = reader.IsDBNull(reader.GetOrdinal("usage")) ? null : reader.GetString(reader.GetOrdinal("usage"));
					barrage.Hauteur = reader.IsDBNull(reader.GetOrdinal("hauteur")) ? 0 : reader.GetInt32(reader.GetOrdinal("hauteur"));
					barrage.Longueur = reader.IsDBNull(reader.GetOrdinal("longueur")) ? 0 : reader.GetInt32(reader.GetOrdinal("longueur"));
					barrage.VolumeRetenu = reader.IsDBNull(reader.GetOrdinal("volume_retenu")) ? 0 : reader.GetInt32(reader.GetOrdinal("volume_retenu"));
					barrage.SurfaceRetenue = reader.IsDBNull(reader.GetOrdinal("surface_retenue")) ? 0 : reader.GetInt32(reader.GetOrdinal("surface_retenue")); ;
					barrage.SurfaceBassin = reader.IsDBNull(reader.GetOrdinal("surface_bassin")) ? 0 : reader.GetInt32(reader.GetOrdinal("surface_bassin"));
					barrage.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? 0 : reader.GetDecimal(reader.GetOrdinal("latitude"));
					barrage.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? 0 : reader.GetDecimal(reader.GetOrdinal("longitude"));
					barrage.Altitude = reader.IsDBNull(reader.GetOrdinal("altitude")) ? 0 : reader.GetDecimal(reader.GetOrdinal("altitude"));

					foreach (Capteur capteur in await CapteurDAO.GetByBarrageAsync(barrage.Id))
					{
						barrage.AddCapteur(capteur);
					}
					foreach (CoteExploitation cote in await CoteExploitationDAO.GetByBarrageAsync(barrage.Id))
					{
						barrage.AddCote(cote);
					}

					connection.Close();
					connection.Dispose();

					return barrage;
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
		public static async Task<List<Barrage>> GetAllAsync()
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				List<Barrage> barrages = new();

				using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id, nom, id_type_barrage, usage, hauteur, longueur, " +
					"volume_retenu, surface_retenue, surface_bassin, latitude, longitude, altitude FROM barrages"
				};

				using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Barrage barrage = new(
							id: reader.GetInt32(reader.GetOrdinal("id")),
							libellé: reader.GetString(reader.GetOrdinal("nom")),
							type: await TypeBarrageDAO.GetByIdAsync(reader.GetInt32(reader.GetOrdinal("id_type_barrage"))));

						barrage.Usage = reader.IsDBNull(reader.GetOrdinal("usage")) ? null : reader.GetString(reader.GetOrdinal("usage"));
						barrage.Hauteur = reader.IsDBNull(reader.GetOrdinal("hauteur")) ? 0 : reader.GetInt32(reader.GetOrdinal("hauteur"));
						barrage.Longueur = reader.IsDBNull(reader.GetOrdinal("longueur")) ? 0 : reader.GetInt32(reader.GetOrdinal("longueur"));
						barrage.VolumeRetenu = reader.IsDBNull(reader.GetOrdinal("volume_retenu")) ? 0 : reader.GetInt32(reader.GetOrdinal("volume_retenu"));
						barrage.SurfaceRetenue = reader.IsDBNull(reader.GetOrdinal("surface_retenue")) ? 0 : reader.GetInt32(reader.GetOrdinal("surface_retenue")); ;
						barrage.SurfaceBassin = reader.IsDBNull(reader.GetOrdinal("surface_bassin")) ? 0 : reader.GetInt32(reader.GetOrdinal("surface_bassin"));
						barrage.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? 0 : reader.GetDecimal(reader.GetOrdinal("latitude"));
						barrage.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? 0 : reader.GetDecimal(reader.GetOrdinal("longitude"));
						barrage.Altitude = reader.IsDBNull(reader.GetOrdinal("altitude")) ? 0 : reader.GetDecimal(reader.GetOrdinal("altitude"));

						foreach (Capteur capteur in await CapteurDAO.GetByBarrageAsync(barrage.Id))
						{
							barrage.AddCapteur(capteur);
						}
						foreach (CoteExploitation cote in await CoteExploitationDAO.GetByBarrageAsync(barrage.Id))
						{
							barrage.AddCote(cote);
						}

						barrages.Add(barrage);
					}

					connection.Close();
					connection.Dispose();

					return barrages;
				}
				else
				{
					return new List<Barrage>();
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de l'accés a la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}

		public static async Task<int> StoreAsync(Barrage barrage)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				if (barrage.IsNew())
				{
					await using NpgsqlCommand commande2 = new()
					{
						Connection = connection,
						CommandText = "INSERT INTO barrages(nom, id_type_barrage, usage, hauteur, longueur, volume_retenu, surface_retenue, surface_bassin, latitude, longitude, altitude) " +
							"values(:nom, :type, :usage, :hauteur, :longueur, :volume_retenu, :surface_retenue, :surface_bassin, :latitude, :longitude, :altitude) returning id;"
					};

					commande2.Parameters.Add(new NpgsqlParameter<string>(":nom", barrage.Libellé ?? ""));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":type", barrage.Type?.Id ?? 0));
					commande2.Parameters.Add(new NpgsqlParameter<string>(":usage", barrage.Usage ?? ""));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":hauteur", barrage.Hauteur));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":longueur", barrage.Longueur));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":volume_retenu", barrage.VolumeRetenu));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":surface_retenue", barrage.SurfaceRetenue));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":surface_bassin", barrage.SurfaceBassin));
					commande2.Parameters.Add(new NpgsqlParameter<decimal>(":latitude", barrage.Latitude));
					commande2.Parameters.Add(new NpgsqlParameter<decimal>(":longitude", barrage.Longitude));
					commande2.Parameters.Add(new NpgsqlParameter<decimal>(":altitude", barrage.Altitude));

					barrage.Id = (short)await commande2.ExecuteNonQueryAsync();

					connection.Close();
					connection.Dispose();

					return 0;
				}
				else
				{
					await using NpgsqlCommand commande2 = new()
					{
						Connection = connection,
						CommandText = "UPDATE barrages SET nom=:nom, id_type_barrage=:type, usage=:usage, hauteur=:hauteur, longueur=:longueur, " +
						"volume_retenu=:volume_retenu, surface_retenue=:surface_retenue, surface_bassin=:surface_bassin, latitude=:latitude, longitude=:longitude, altitude=:altitude WHERE id=" + barrage.Id
					};

					commande2.Parameters.Add(new NpgsqlParameter<string>(":nom", barrage.Libellé ?? ""));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":type", barrage.Type?.Id ?? 0));
					commande2.Parameters.Add(new NpgsqlParameter<string>(":usage", barrage.Usage ?? ""));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":hauteur", barrage.Hauteur));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":longueur", barrage.Longueur));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":volume_retenu", barrage.VolumeRetenu));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":surface_retenue", barrage.SurfaceRetenue));
					commande2.Parameters.Add(new NpgsqlParameter<int>(":surface_bassin", barrage.SurfaceBassin));
					commande2.Parameters.Add(new NpgsqlParameter<decimal>(":latitude", barrage.Latitude));
					commande2.Parameters.Add(new NpgsqlParameter<decimal>(":longitude", barrage.Longitude));
					commande2.Parameters.Add(new NpgsqlParameter<decimal>(":altitude", barrage.Altitude));

					await commande2.ExecuteNonQueryAsync();

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
