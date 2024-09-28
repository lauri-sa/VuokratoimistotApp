using System;
using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    internal class InvoiceRepository
    {
        /// <summary>
        /// Retrieves a collection of invoices based on the given customer ID, start date, and end date.
        /// </summary>
        /// <param name="customerID">The ID of the customer.</param>
        /// <param name="startDate">The start date of the invoice period.</param>
        /// <param name="endDate">The end date of the invoice period.</param>
        /// <returns>An observable collection of ViewInvoiceModel objects representing the invoices.</returns>
        public static async Task<ObservableCollection<ViewInvoiceModel>> FetchInvoices(int customerID, DateTime startDate, DateTime endDate)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT i.InvoiceID, i.Date, i.DueDate, i.TotalSum, i.ReservationID,
                                                  c.FirstName, c.LastName, c.Email, c.StreetAddress, c.Postalcode, c.City,
                                                  r.StartDate, r.EndDate, ro.Price, os.Name
                                           FROM Invoice i
                                           JOIN Customer c ON c.CustomerID = i.CustomerID
                                           JOIN Reservation r ON r.ReservationID = i.ReservationID
                                           JOIN ReservationOffice ro ON ro.ReservationID = r.ReservationID
                                           JOIN OfficeSpace os ON os.SpaceID = ro.SpaceID
                                           WHERE i.Date >= @StartDate AND i.Date <= @EndDate
                                           AND i.CustomerID = @CustomerID
                                           ORDER BY i.Date;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var invoicesCollection = new ObservableCollection<ViewInvoiceModel>();

                        while (await reader.ReadAsync())
                        {
                            var viewInvoiceModel = new ViewInvoiceModel
                            {
                                ID = (int)reader["InvoiceID"],
                                ReservationID = (int)reader["ReservationID"],
                                CustomerName = $"{reader["FirstName"]} {reader["LastName"]}",
                                CustomerEmail = (string)reader["Email"],
                                StreetAddress = (string)reader["StreetAddress"],
                                PostalCode = (string)reader["Postalcode"],
                                City = (string)reader["City"],
                                Date = ((DateTime)reader["Date"]).ToShortDateString(),
                                DueDate = ((DateTime)reader["DueDate"]).ToShortDateString(),
                                TotalSum = reader["TotalSum"].ToString(),
                                InvoiceRows = new ObservableCollection<InvoiceRowModel>()
                            };

                            var amount = (int)((DateTime)reader["EndDate"]).Date.Subtract(((DateTime)reader["StartDate"]).Date).TotalDays;

                            amount = amount < 1 ? 1 : amount;

                            viewInvoiceModel.InvoiceRows.Add(new InvoiceRowModel
                            {
                                ProductName = (string)reader["Name"],
                                ProductPrice = (Math.Round(((float)reader["Price"] / amount), 2)).ToString(),
                                Amount = amount.ToString(),
                                TotalSum = reader["Price"].ToString()
                            });

                            invoicesCollection.Add(viewInvoiceModel);
                        }

                        return invoicesCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches the invoice rows for each invoice in the provided ObservableCollection.
        /// </summary>
        /// <param name="invoicesCollection">The ObservableCollection of ViewInvoiceModels for which to fetch the invoice rows.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public static async Task FetchInvoiceRows(ObservableCollection<ViewInvoiceModel> invoicesCollection)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT r.StartDate, r.EndDate, rs.Price, s.Name
                                           FROM Reservation r
                                           JOIN ReservationService rs ON rs.ReservationID = r.ReservationID
                                           JOIN AdditionalService s ON s.ServiceID = rs.ServiceID
                                           WHERE r.ReservationID = @ReservationID;";

                foreach (var invoice in invoicesCollection)
                {
                    using (var command = new MySqlCommand(STATEMENT, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", invoice.ReservationID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var amount = (int)((DateTime)reader["EndDate"]).Date.Subtract(((DateTime)reader["StartDate"]).Date).TotalDays;

                                amount = amount < 1 ? 1 : amount;

                                invoice.InvoiceRows.Add(new InvoiceRowModel
                                {
                                    ProductName = (string)reader["Name"],
                                    ProductPrice = (Math.Round(((float)reader["Price"] / amount), 2)).ToString(),
                                    Amount = amount.ToString(),
                                    TotalSum = reader["Price"].ToString()
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}