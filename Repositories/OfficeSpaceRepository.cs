using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    /// <summary>
    /// A repository class for managing office spaces. It provides methods for fetching, adding,
    /// updating, and deleting office spaces from a MySQL database.
    /// </summary>
    internal class OfficeSpaceRepository
    {

        /// <summary>
        /// Retrieves a collection of office spaces with their IDs, names and prices from the database asynchronously.
        /// </summary>
        /// <param name="officeID">The ID of the office for which to fetch office spaces.</param>
        /// <returns>An ObservableCollection of OfficeSpaceModel objects.</returns>
        public static async Task<ObservableCollection<OfficeSpaceModel>> FetchAllOfficeSpacesMinorDetail(int officeID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT SpaceID, Name, Price
                                           FROM OfficeSpace
                                           WHERE OfficeID = @OfficeID";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", officeID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var officeSpaceCollection = new ObservableCollection<OfficeSpaceModel>();

                        while (await reader.ReadAsync())
                        {
                            officeSpaceCollection.Add(new OfficeSpaceModel
                            {
                                ID = (int)reader["SpaceID"],
                                Name = (string)reader["Name"],
                                Price = reader["Price"].ToString()
                            });
                        }

                        return officeSpaceCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a collection of office spaces from the database asynchronously.
        /// </summary>
        /// <param name="officeID">The ID of the office for which to fetch office spaces.</param>
        /// <returns>An ObservableCollection of OfficeSpaceModel objects.</returns>
        public static async Task<ObservableCollection<OfficeSpaceModel>> FetchAllOfficeSpacesFullDetail(int officeID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT *
                                           FROM OfficeSpace
                                           WHERE OfficeID = @OfficeID";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", officeID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var officeSpaceCollection = new ObservableCollection<OfficeSpaceModel>();

                        while (await reader.ReadAsync())
                        {
                            officeSpaceCollection.Add(new OfficeSpaceModel
                            {
                                ID = (int)reader["SpaceID"],
                                OfficeID = (int)reader["OfficeID"],
                                Name = (string)reader["Name"],
                                Price = reader["Price"].ToString(),
                                Capacity = reader["Capacity"].ToString(),
                                Size = reader["Size"].ToString()
                            });
                        }

                        return officeSpaceCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new office space to the database.
        /// </summary>
        /// <param name="officeSpaceModel">The office space to be added.</param>
        public static void AddOfficeSpace(OfficeSpaceModel officeSpaceModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"INSERT INTO OfficeSpace (OfficeID, Name, Price, Capacity, Size)
                                           VALUES (@OfficeID, @Name, @Price, @Capacity, @Size);COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", officeSpaceModel.OfficeID);
                    command.Parameters.AddWithValue("@Name", officeSpaceModel.Name);
                    command.Parameters.AddWithValue("@Price", float.Parse(officeSpaceModel.Price));
                    command.Parameters.AddWithValue("@Capacity", int.Parse(officeSpaceModel.Capacity));
                    command.Parameters.AddWithValue("@Size", int.Parse(officeSpaceModel.Size));

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates the given office space in the database.
        /// </summary>
        /// <param name="officeSpaceModel">The OfficeSpaceModel object containing the details of the office space to be updated.</param>
        public static void UpdateOfficeSpace(OfficeSpaceModel officeSpaceModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"UPDATE OfficeSpace
                                           SET Name = @Name, Price = @Price, Capacity = @Capacity, Size = @Size
                                           WHERE SpaceID = @SpaceID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceModel.ID);
                    command.Parameters.AddWithValue("@Name", officeSpaceModel.Name);
                    command.Parameters.AddWithValue("@Price", float.Parse(officeSpaceModel.Price));
                    command.Parameters.AddWithValue("@Capacity", int.Parse(officeSpaceModel.Capacity));
                    command.Parameters.AddWithValue("@Size", int.Parse(officeSpaceModel.Size));

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a office space with the specified ID from the database.
        /// </summary>
        /// <param name="officeSpaceID">The ID of the office space to be deleted.</param>
        public static void DeleteOfficeSpace(int officeSpaceID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"DELETE
                                           FROM OfficeSpace
                                           WHERE SpaceID = @SpaceID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}