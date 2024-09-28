using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    /// <summary>
    /// A repository class for managing offices. It provides methods for fetching, adding,
    /// updating, and deleting offices from a MySQL database.
    /// </summary>
    internal class OfficeRepository
    {
        /// <summary>
        /// Retrieves a collection of all offices with their IDs and names from the database asynchronously.
        /// </summary>
        /// <returns>An ObservableCollection of OfficeModel objects containing all offices in the database.</returns>
        public static async Task<ObservableCollection<OfficeModel>> FetchAllOfficesMinorDetails()
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT OfficeID, OfficeName
                                           FROM Office;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var officeCollection = new ObservableCollection<OfficeModel>();

                        while (await reader.ReadAsync())
                        {
                            officeCollection.Add(new OfficeModel
                            {
                                ID = (int)reader["OfficeID"],
                                OfficeName = (string)reader["OfficeName"]
                            });
                        }

                        return officeCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a collection of all offices from the database asynchronously.
        /// </summary>
        /// <returns>An ObservableCollection of OfficeModel objects containing all offices in the database.</returns>
        public static async Task<ObservableCollection<OfficeModel>> FetchAllOfficesFullDetails()
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT *
                                           FROM Office;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var officeCollection = new ObservableCollection<OfficeModel>();

                        while (await reader.ReadAsync())
                        {
                            officeCollection.Add(new OfficeModel
                            {
                                ID = (int)reader["OfficeID"],
                                OfficeName = (string)reader["OfficeName"],
                                StreetAddress = (string)reader["StreetAddress"],
                                City = (string)reader["City"],
                                PostalCode = (string)reader["Postalcode"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                Email = (string)reader["Email"]
                            });
                        }

                        return officeCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new office to the database.
        /// </summary>
        /// <param name="officeModel">The office to be added.</param>
        public static void AddOffice(OfficeModel officeModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"INSERT INTO Office (OfficeName, StreetAddress, Postalcode, City, PhoneNumber, Email)
                                           VALUES (@OfficeName, @StreetAddress, @Postalcode, @City, @PhoneNumber, @Email);COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeName", officeModel.OfficeName);
                    command.Parameters.AddWithValue("@StreetAddress", officeModel.StreetAddress);
                    command.Parameters.AddWithValue("@Postalcode", officeModel.PostalCode);
                    command.Parameters.AddWithValue("@City", officeModel.City);
                    command.Parameters.AddWithValue("@PhoneNumber", officeModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", officeModel.Email);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates the given office in the database.
        /// </summary>
        /// <param name="officeModel">The OfficeModel object containing the details of the office to be updated</param>
        public static void UpdateOffice(OfficeModel officeModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"UPDATE Office
                                           SET OfficeName = @OfficeName, StreetAddress = @StreetAddress, City = @City,
                                               PostalCode = @PostalCode, PhoneNumber = @PhoneNumber, Email = @Email
                                           WHERE OfficeID = @OfficeID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", officeModel.ID);
                    command.Parameters.AddWithValue("@OfficeName", officeModel.OfficeName);
                    command.Parameters.AddWithValue("@StreetAddress", officeModel.StreetAddress);
                    command.Parameters.AddWithValue("@City", officeModel.City);
                    command.Parameters.AddWithValue("@PostalCode", officeModel.PostalCode);
                    command.Parameters.AddWithValue("@PhoneNumber", officeModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", officeModel.Email);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a office with the specified ID from the database.
        /// </summary>
        /// <param name="officeID">The ID of the office to be deleted.</param>
        public static void DeleteOffice(int officeID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"DELETE
                                           FROM Office
                                           WHERE OfficeID = @OfficeID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", officeID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}