using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    /// <summary>
    /// A repository class for managing customers. It provides methods for fetching, adding,
    /// updating, and deleting customers from a MySQL database.
    /// </summary>
    internal class CustomerRepository
    {
        /// <summary>
        /// Retrieves ID and name of all customers from the database asynchronously and returns them as an ObservableCollection..
        /// </summary>
        /// <returns>An ObservableCollection of CustomerModel objects containing all customers in the database.</returns>
        public static async Task<ObservableCollection<CustomerModel>> FetchAllCustomersMinorDetails()
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT CustomerID, FirstName, LastName
                                           FROM Customer;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var customerCollection = new ObservableCollection<CustomerModel>();

                        while (await reader.ReadAsync())
                        {
                            customerCollection.Add(new CustomerModel
                            {
                                ID = (int)reader["CustomerID"],
                                FullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}",
                            });
                        }

                        return customerCollection;
                    }
                }
            }
        }


        /// <summary>
        /// Retrieves all customers from the database asynchronously and returns them as an ObservableCollection..
        /// </summary>
        /// <returns>An ObservableCollection of CustomerModel objects containing all customers in the database.</returns>
        public static async Task<ObservableCollection<CustomerModel>> FetchAllCustomers()
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT *
                                           FROM Customer;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var customerCollection = new ObservableCollection<CustomerModel>();

                        while (await reader.ReadAsync())
                        {
                            customerCollection.Add(new CustomerModel
                            {
                                ID = (int)reader["CustomerID"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                FullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}",
                                Email = (string)reader["Email"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                StreetAddress = (string)reader["StreetAddress"],
                                PostalCode = (string)reader["Postalcode"],
                                City = (string)reader["City"]
                            });
                        }

                        return customerCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        /// <param name="customerModel">The customer to be added.</param>
        public static void AddCustomer(CustomerModel customerModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"INSERT INTO Customer (FirstName, LastName, StreetAddress, Postalcode, City, PhoneNumber, Email)
                                           VALUES (@FirstName, @LastName, @StreetAddress, @Postalcode, @City, @PhoneNumber, @Email);COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", customerModel.FirstName);
                    command.Parameters.AddWithValue("@LastName", customerModel.LastName);
                    command.Parameters.AddWithValue("@StreetAddress", customerModel.StreetAddress);
                    command.Parameters.AddWithValue("@Postalcode", customerModel.PostalCode);
                    command.Parameters.AddWithValue("@City", customerModel.City);
                    command.Parameters.AddWithValue("@PhoneNumber", customerModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", customerModel.Email);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates the given customer in the database.
        /// </summary>
        /// <param name="customerModel">The CustomerModel object containing the details of the customer to be updated</param>
        public static void UpdateCustomer(CustomerModel customerModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"UPDATE Customer
                                           SET FirstName = @FirstName, LastName = @LastName, StreetAddress = @StreetAddress,
                                               Postalcode = @Postalcode, City = @City, PhoneNumber = @PhoneNumber, Email = @Email
                                           WHERE CustomerID = @CustomerID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerModel.ID);
                    command.Parameters.AddWithValue("@FirstName", customerModel.FirstName);
                    command.Parameters.AddWithValue("@LastName", customerModel.LastName);
                    command.Parameters.AddWithValue("@StreetAddress", customerModel.StreetAddress);
                    command.Parameters.AddWithValue("@Postalcode", customerModel.PostalCode);
                    command.Parameters.AddWithValue("@City", customerModel.City);
                    command.Parameters.AddWithValue("@PhoneNumber", customerModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", customerModel.Email);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a customer with the specified ID from the database.
        /// </summary>
        /// <param name="customerID">The ID of the customer to be deleted.</param>
        public static void DeleteCustomer(int customerID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"DELETE FROM Customer
                                           WHERE CustomerID = @CustomerID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}