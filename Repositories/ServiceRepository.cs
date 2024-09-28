using System;
using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    /// <summary>
    /// A repository class for managing services. It provides methods for fetching, adding,
    /// updating, and deleting services from a MySQL database.
    /// </summary>
    internal class ServiceRepository
    {
        /// <summary>
        /// Fetches all the services associated with a given office space.
        /// </summary>
        /// <param name="officeSpaceID">The ID of the office space for which to fetch services.</param>
        /// <returns>An ObservableCollection of ServiceModel objects representing the services associated with the specified office space.</returns>
        public static async Task<ObservableCollection<ServiceModel>> FetchAllServicesMinorDetails(int officeSpaceID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT ServiceID, Name, Price
                                           FROM AdditionalService
                                           WHERE SpaceID = @SpaceID";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var serviceCollection = new ObservableCollection<ServiceModel>();

                        while (await reader.ReadAsync())
                        {
                            serviceCollection.Add(new ServiceModel
                            {
                                ID = (int)reader["ServiceID"],
                                Name = (string)reader["Name"],
                                Price = reader["Price"].ToString()
                            });
                        }

                        return serviceCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches all the services associated with a given office asynchronously.
        /// </summary>
        /// <param name="OfficeID">The ID of the office for which to fetch services.</param>
        /// <returns>An ObservableCollection of ServiceModel objects representing the services associated with the specified office.</returns>
        public static async Task<ObservableCollection<ServiceModel>> FetchAllOfficeServices(int OfficeID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT s.ServiceID, s.SpaceID, s.Name, s.Description, s.Price, os.Name AS SpaceName
                                           FROM AdditionalService s
                                           JOIN OfficeSpace os ON os.SpaceID = s.SpaceID
                                           JOIN Office o ON o.OfficeID = os.OfficeID
                                           WHERE o.OfficeID = @OfficeID;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", OfficeID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var serviceCollection = new ObservableCollection<ServiceModel>();

                        while (await reader.ReadAsync())
                        {
                            serviceCollection.Add(new ServiceModel
                            {
                                ID = (int)reader["ServiceID"],
                                SpaceID = (int)reader["SpaceID"],
                                SpaceName = (string)reader["SpaceName"],
                                Name = (string)reader["Name"],
                                Description = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"],
                                Price = reader["Price"].ToString()
                            });
                        }

                        return serviceCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new service to the database.
        /// </summary>
        /// <param name="officeSpaceID">ID of the officespace that service is added to.</param>
        /// <param name="serviceModel">The service to be added.</param>
        public static void AddService(int officeSpaceID, ServiceModel serviceModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"INSERT INTO AdditionalService (SpaceID, Name, Description, Price)
                                           VALUES (@SpaceID, @Name, @Description, @Price);COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceID);
                    command.Parameters.AddWithValue("@Name", serviceModel.Name);
                    command.Parameters.AddWithValue("@Description", serviceModel.Description);
                    command.Parameters.AddWithValue("@Price", float.Parse(serviceModel.Price));

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates the given service in the database.
        /// </summary>
        /// <param name="serviceModel">The ServiceModel object containing the details of the service to be updated</param>
        public static void UpdateService(ServiceModel serviceModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"UPDATE AdditionalService
                                           SET Name = @Name, Description = @Description, Price = @Price
                                           WHERE ServiceID = @ServiceID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@ServiceID", serviceModel.ID);
                    command.Parameters.AddWithValue("@Name", serviceModel.Name);
                    command.Parameters.AddWithValue("@Description", serviceModel.Description);
                    command.Parameters.AddWithValue("@Price", serviceModel.Price);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a service with the specified ID from the database.
        /// </summary>
        /// <param name="serviceID">The ID of the service to be deleted.</param>
        public static void DeleteService(int serviceID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"DELETE
                                           FROM AdditionalService
                                           WHERE ServiceID = @ServiceID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@ServiceID", serviceID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}