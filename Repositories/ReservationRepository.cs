using System;
using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    internal class ReservationRepository
    {
        /// <summary>
        /// Adds a reservation and invoice to the database using the given ReservationModel and InvoiceModel objects.
        /// A transaction is used to ensure that both the reservation and invoice are added atomically, i.e. if one fails,
        /// the entire transaction is rolled back.
        /// </summary>
        /// <param name="reservationModel">The ReservationModel object containing the reservation details to be added.</param>
        /// <param name="invoiceModel">The InvoiceModel object containing the invoice details to be added.</param>
        public static void AddReservation(ReservationModel reservationModel, InvoiceModel invoiceModel)
        {
            long reservationID;

            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string RESERVATION_STATEMENT = @"INSERT INTO Reservation (CustomerID, StartDate, EndDate, ReservationDay)
                                                   VALUES (@CustomerID, @StartDate, @EndDate, @ReservationDay)";

                        using (var command = new MySqlCommand(RESERVATION_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CustomerID", reservationModel.CustomerID);
                            command.Parameters.AddWithValue("@StartDate", reservationModel.StartDate);
                            command.Parameters.AddWithValue("@EndDate", reservationModel.EndDate);
                            command.Parameters.AddWithValue("@ReservationDay", reservationModel.ReservationDay);

                            command.ExecuteNonQuery();

                            reservationID = command.LastInsertedId;
                        }

                        const string RESERVATIONOFFICE_STATEMENT = @"INSERT INTO ReservationOffice (ReservationID, SpaceID, Price)
                                                                     VALUES (@ReservationID, @SpaceID, @Price)";

                        using (var command = new MySqlCommand(RESERVATIONOFFICE_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ReservationID", reservationID);
                            command.Parameters.AddWithValue("@SpaceID", reservationModel.SpaceID);
                            command.Parameters.AddWithValue("@Price", Math.Round(reservationModel.OfficeSpacePrice, 2));

                            command.ExecuteNonQuery();
                        }

                            const string RESERVATIONSERVICE_STATEMENT = @"INSERT INTO ReservationService (ReservationID, ServiceID, Price)
                                                                          VALUES (@ReservationID, @ServiceID, @Price)";

                        foreach (var service in reservationModel.ReservationServices)
                        {
                            using (var command = new MySqlCommand(RESERVATIONSERVICE_STATEMENT, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@ReservationID", reservationID);
                                command.Parameters.AddWithValue("@ServiceID", service.ID);
                                command.Parameters.AddWithValue("@Price", Math.Round(float.Parse(service.Price), 2));

                                command.ExecuteNonQuery();
                            }
                        }

                        const string INVOICE_STATEMENT = @"INSERT INTO Invoice (ReservationID, CustomerID, Date, DueDate, TotalSum)
                                                           VALUES (@ReservationID, @CustomerID, @Date, @DueDate, @TotalSum)";

                        using (var command = new MySqlCommand(INVOICE_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ReservationID", reservationID);
                            command.Parameters.AddWithValue("@CustomerID", invoiceModel.CustomerID);
                            command.Parameters.AddWithValue("@Date", invoiceModel.Date);
                            command.Parameters.AddWithValue("@DueDate", invoiceModel.DueDate);
                            command.Parameters.AddWithValue("@TotalSum", Math.Round(invoiceModel.TotalSum, 2));

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Updates a reservation and invoice to the database using the given ReservationModel and InvoiceModel objects.
        /// A transaction is used to ensure that both the reservation and invoice are added atomically, i.e. if one fails,
        /// the entire transaction is rolled back.
        /// </summary>
        /// <param name="reservationModel">The ReservationModel object containing the reservation details to be added.</param>
        /// <param name="invoiceModel">The InvoiceModel object containing the invoice details to be added.</param>
        public static void UpdateReservation(ReservationModel reservationModel, InvoiceModel invoiceModel)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string RESERVATION_STATEMENT = @"UPDATE Reservation SET CustomerID = @CustomerID, StartDate = @StartDate,
                                                                                      EndDate = @EndDate, ReservationDay = @ReservationDay
                                                               WHERE ReservationID = @ReservationID";

                        using (var command = new MySqlCommand(RESERVATION_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CustomerID", reservationModel.CustomerID);
                            command.Parameters.AddWithValue("@StartDate", reservationModel.StartDate);
                            command.Parameters.AddWithValue("@EndDate", reservationModel.EndDate);
                            command.Parameters.AddWithValue("@ReservationDay", reservationModel.ReservationDay);
                            command.Parameters.AddWithValue("@ReservationID", reservationModel.ID);

                            command.ExecuteNonQuery();
                        }

                        const string RESERVATIONOFFICE_STATEMENT = @"UPDATE ReservationOffice SET SpaceID = @SpaceID, Price = @Price
                                                                     WHERE ReservationID = @ReservationID";

                        using (var command = new MySqlCommand(RESERVATIONOFFICE_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@SpaceID", reservationModel.SpaceID);
                            command.Parameters.AddWithValue("@Price", Math.Round(reservationModel.OfficeSpacePrice, 2));
                            command.Parameters.AddWithValue("@ReservationID", reservationModel.ID);

                            command.ExecuteNonQuery();
                        }

                        const string RESERVATIONSERVICE_DELETE_STATEMENT = @"DELETE FROM ReservationService
                                                                             WHERE ReservationID = @ReservationID";

                        using (var command = new MySqlCommand(RESERVATIONSERVICE_DELETE_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ReservationID", reservationModel.ID);

                            command.ExecuteNonQuery();
                        }

                        const string RESERVATIONSERVICE_STATEMENT = @"INSERT INTO ReservationService (ReservationID, ServiceID, Price)
                                                                      VALUES (@ReservationID, @ServiceID, @Price)";

                        foreach (var service in reservationModel.ReservationServices)
                        {
                            using (var command = new MySqlCommand(RESERVATIONSERVICE_STATEMENT, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@ReservationID", reservationModel.ID);
                                command.Parameters.AddWithValue("@ServiceID", service.ID);
                                command.Parameters.AddWithValue("@Price", Math.Round(float.Parse(service.Price), 2));

                                command.ExecuteNonQuery();
                            }
                        }

                        const string INVOICE_STATEMENT = @"UPDATE Invoice SET CustomerID = @CustomerID, Date = @Date,
                                                                              DueDate = @DueDate, TotalSum = @TotalSum
                                                           WHERE ReservationID = @ReservationID";

                        using (var command = new MySqlCommand(INVOICE_STATEMENT, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CustomerID", invoiceModel.CustomerID);
                            command.Parameters.AddWithValue("@Date", invoiceModel.Date);
                            command.Parameters.AddWithValue("@DueDate", invoiceModel.DueDate);
                            command.Parameters.AddWithValue("@TotalSum", Math.Round(invoiceModel.TotalSum, 2));
                            command.Parameters.AddWithValue("@ReservationID", reservationModel.ID);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Fetches a collection of reservations from the database asynchronously for the specified customer ID, start date, and end date.
        /// </summary>
        /// <param name="customerID">The ID of the customer for which to fetch reservations.</param>
        /// <param name="startDate">The start date of the reservation period.</param>
        /// <param name="endDate">The end date of the reservation period.</param>
        /// <returns>An ObservableCollection of ReservationModel objects representing the reservations that match the search criteria.</returns>
        public static async Task<ObservableCollection<ReservationModel>> FetchReservations(int customerID, DateTime startDate, DateTime endDate)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT r.ReservationID, r.CustomerID, r.StartDate, r.EndDate,
                                                  r.ReservationDay, ro.SpaceID, os.OfficeID, c.FirstName, c.LastName
                                           FROM Reservation r
                                           JOIN ReservationOffice ro ON ro.ReservationID = r.ReservationID
                                           JOIN OfficeSpace os ON os.SpaceID = ro.SpaceID
                                           JOIN Customer c ON c.CustomerID = r.CustomerID
                                           WHERE r.StartDate >= @StartDate AND r.EndDate <= @EndDate
                                           AND r.CustomerID = @CustomerID";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var reservationsCollection = new ObservableCollection<ReservationModel>();

                        while(await reader.ReadAsync())
                        {
                            var reservationModel = new ReservationModel
                            {
                                ID = (int)reader["ReservationID"],
                                CustomerID = (int)reader["CustomerID"],
                                OfficeID = (int)reader["OfficeID"],
                                SpaceID = (int)reader["SpaceID"],
                                CustomerName = $"{reader["FirstName"]} {reader["LastName"]}",
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = (DateTime)reader["EndDate"],
                                ReservationDay = ((DateTime)reader["ReservationDay"]).Date.ToShortDateString(),
                                ReservationServices = new ObservableCollection<ServiceModel>()
                            };

                            reservationsCollection.Add(reservationModel);
                        }

                        return reservationsCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches the additional services for each reservation in the given collection.
        /// </summary>
        /// <param name="reservationCollection">The collection of ReservationModels to update.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task FetchReservationServices(ObservableCollection<ReservationModel> reservationCollection)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT s.ServiceID, s.Name, s.Price
                                           FROM ReservationService rs
                                           JOIN AdditionalService s ON s.ServiceID = rs.ServiceID
                                           WHERE ReservationID = @ReservationID;";

                foreach (var reservation in reservationCollection)
                {
                    using (var command = new MySqlCommand(STATEMENT, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", reservation.ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var serviceModel = new ServiceModel
                                {
                                    ID = (int)reader["ServiceID"],
                                    Name = (string)reader["Name"],
                                    Price = reader["Price"].ToString()
                                };

                                reservation.ReservationServices.Add(serviceModel);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fetches the start and end dates of all reservations for a given office space ID from the database.
        /// </summary>
        /// <param name="officeSpaceID">The ID of the office space for which to fetch the reservations.</param>
        /// <returns>A list of CalendarDateRange objects representing the start and end dates of each reservation.</returns>
        public static List<DateTime> FetchReservationDates(int officeSpaceID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"SELECT r.StartDate, r.EndDate
                                           FROM Reservation r
                                           JOIN ReservationOffice ro ON ro.ReservationID = r.ReservationID
                                           WHERE ro.SpaceID = @SpaceID";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceID);

                    using (var reader = command.ExecuteReader())
                    {
                        var reservationDatesCollection = new List<DateTime>();

                        while (reader.Read())
                        {
                            var dateRange = ((DateTime)reader["EndDate"]).Date - ((DateTime)reader["StartDate"]).Date;

                            for (int i = 0; i <= dateRange.TotalDays; i++)
                            {
                                reservationDatesCollection.Add(((DateTime)reader["StartDate"]).AddDays(i));
                            };
                        }

                        return reservationDatesCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches the start and end dates of all reservations for a given office space ID from the database excluding dates between startdate and enddate.
        /// </summary>
        /// <param name="officeSpaceID">The ID of the office space for which to fetch the reservations.</param>
        /// <param name="startDate">Startdate of the current reservation.</param>
        /// <param name="endDate">EndDate of the current reservation.</param>
        /// <returns>A list of CalendarDateRange objects representing the start and end dates of each reservation.</returns>
        public static List<DateTime> FetchReservationDates(int officeSpaceID, DateTime startDate, DateTime endDate)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"SELECT r.StartDate, r.EndDate
                                           FROM Reservation r
                                           JOIN ReservationOffice ro ON ro.ReservationID = r.ReservationID
                                           WHERE ro.SpaceID = @SpaceID
                                           AND r.StartDate != @StartDate
                                           AND r.EndDate != @EndDate";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceID);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        var reservationDatesCollection = new List<DateTime>();

                        while (reader.Read())
                        {
                            var dateRange = ((DateTime)reader["EndDate"]).Date - ((DateTime)reader["StartDate"]).Date;

                            for (int i = 0; i <= dateRange.TotalDays; i++)
                            {
                                reservationDatesCollection.Add(((DateTime)reader["StartDate"]).AddDays(i));
                            };
                        }

                        return reservationDatesCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a reservation with the specified ID from the database.
        /// </summary>
        /// <param name="officeID">The ID of the reservation to be deleted.</param>
        public static void DeleteReservation(int reservationID)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                connection.Open();

                const string STATEMENT = @"DELETE
                                           FROM Reservation
                                           WHERE ReservationID = @ReservationID;COMMIT;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", reservationID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}