using Model;
using Npgsql;

namespace API.DAL
{
	public class TypeBarrageDAO
	{
		public static async Task<TypeBarrage> GetByIdAsync(int id)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				await using NpgsqlCommand commande = new()
				{
					Connection = connection,
					CommandText = "SELECT id, libelle FROM types_barrage t where t.id = :id;"
				};

				commande.Parameters.Add(new NpgsqlParameter<int>("id", id));

				await using NpgsqlDataReader reader = commande.ExecuteReader();

				if (reader.HasRows)
				{
					await reader.ReadAsync();

					TypeBarrage type = new(
						id: reader.GetInt32(reader.GetOrdinal("id")),
						libellé: reader.GetString(reader.GetOrdinal("libelle")));

					connection.Close();
					connection.Dispose();

					return type;

				}
				else
				{
					throw new ApplicationException("Le type de barrage demandé n'existe pas.");
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de la connection à la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}

		public static async Task<List<TypeBarrage>> GetAllAsync()
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				List<TypeBarrage> types = new();

				await using NpgsqlCommand command = new()
				{
					Connection = connection,
					CommandText = "SELECT id, libelle FROM types_barrage;"
				};

				await using NpgsqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (await reader.ReadAsync())
					{
						TypeBarrage type = new(
							id: reader.GetInt32(reader.GetOrdinal("id")),
							libellé: reader.GetString(reader.GetOrdinal("libelle")));

						types.Add(type);

					}

					connection.Close();
					connection.Dispose();

					return types;
				}
				else
				{
					return new List<TypeBarrage>();
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erreur lors de la connection à la base de données\n" + ex.Message + "\n" + ex.InnerException, ex);
			}
		}

		public static async Task<int> StoreAsync(TypeBarrage type)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				if (type.IsNew())
				{
					await using NpgsqlCommand commande = new()
					{
						Connection = connection,
						CommandText = "INSERT INTO types_barrage(libelle) values(:libellé) returning id;"
					};

					commande.Parameters.Add(new NpgsqlParameter<string>(":libellé", type.Libellé ?? ""));

					type.Id = (short)(await commande.ExecuteScalarAsync())!;

					connection.Close();
					connection.Dispose();

					return 0;
				}
				else
				{
					await using NpgsqlCommand commande = new()
					{
						Connection = connection,
						CommandText = "UPDATE types_barrage SET libelle=:libellé WHERE id=" + type.Id
					};

					commande.Parameters.Add(new NpgsqlParameter<string>(":libellé", type.Libellé ?? ""));

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
