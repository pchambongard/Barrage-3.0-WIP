using Barrage_Model;
using Npgsql;

namespace Service_Température.DAL
{
	public class MesureTempératureDAL
	{
		public static async Task<int> StoreAsync(MesureTempérature mesure)
		{
			try
			{
				await using NpgsqlConnection connection = new("Server=srw-pgtest;Port=5432;Database=barrage;User Id=pgbarrage;Password=DbaBarrage!30;Pooling=false");

				await connection.OpenAsync().ConfigureAwait(false);

				string commande = "INSERT INTO mesuresTemperature(id_capteur, gdh, valeur) values(:id_capteur, :gdh, :valeur)";



				await using NpgsqlCommand command = new()
				{
					Connection = connection,
					CommandText = commande
				};

				command.Parameters.Add(new NpgsqlParameter<int>(":id_capteur", mesure.IdCapteur));
				command.Parameters.Add(new NpgsqlParameter<DateTime>(":gdh", mesure.Gdh));
				command.Parameters.Add(new NpgsqlParameter<decimal>(":valeur", mesure.Valeur));
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
