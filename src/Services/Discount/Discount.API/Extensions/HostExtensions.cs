using Npgsql;

namespace Discount.API.Extensions
{
	public static class HostExtensions
	{
		public static IHost MigrateDatabase<T>(this IHost host, int? retry = 0)
		{
			int retryForAvailability = retry.Value;
			using var scope = host.Services.CreateScope();

			var serviceCollection = scope.ServiceProvider;
			var config = serviceCollection.GetRequiredService<IConfiguration>();
			var logger = serviceCollection.GetRequiredService<ILogger<T>>();

			try
			{
				logger.LogInformation("Migration postresql database");

				using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
				connection.Open();

				using var command = new NpgsqlCommand() { Connection =  connection };

				command.CommandText = "DROP TABLE IF EXISTS Coupon";
				command.ExecuteNonQuery();

				command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
				command.ExecuteNonQuery();

				command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
				command.ExecuteNonQuery();

				command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
				command.ExecuteNonQuery();

				logger.LogInformation("Migrated postresql database.");
			}
			catch (NpgsqlException ex)
			{
				logger.LogError(ex.ToString());
				if(retryForAvailability < 50) 
				{
					retryForAvailability++;
					Thread.Sleep(2000);
					MigrateDatabase<T>(host, retryForAvailability);
				}

			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
			}

			return host;
		}
	}
}
