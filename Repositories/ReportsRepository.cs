using System;
using MySqlConnector;
using Ohtu1Project.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ohtu1Project.Repositories
{
    internal class ReportsRepository
    {
        /// <summary>
        /// Fetches the reservation reports for a given office within a specific date range.
        /// </summary>
        /// <param name="officeID">The ID of the office to fetch the reports for.</param>
        /// <param name="startDate">The start date of the date range.</param>
        /// <param name="endDate">The end date of the date range.</param>
        /// <returns>An ObservableCollection of OfficeReportModel containing the reservation reports.</returns>
        public static async Task<ObservableCollection<OfficeReportModel>> FetchOfficeReports(int officeID, DateTime startDate, DateTime endDate)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT r.ReservationID, r.ReservationDay, r.StartDate, r.EndDate,
                                                  c.FirstName, c.LastName, c.PhoneNumber, c.Email,
                                                  os.Name AS OfficeSpaceName, o.OfficeName
                                           FROM Reservation r
                                           JOIN Customer c ON c.CustomerID = r.CustomerID
                                           JOIN ReservationOffice ro ON ro.ReservationID = r.ReservationID
                                           JOIN OfficeSpace os ON os.SpaceID = ro.SpaceID
                                           JOIN Office o ON o.OfficeID = os.OfficeID
                                           WHERE r.StartDate >= @StartDate AND r.EndDate <= @EndDate
                                           AND o.OfficeID = @OfficeID
                                           ORDER BY r.StartDate;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@OfficeID", officeID);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var officeReportCollection = new ObservableCollection<OfficeReportModel>();

                        while (await reader.ReadAsync())
                        {
                            var officeReportModel = new OfficeReportModel
                            {
                                ReservationID = (int)reader["ReservationID"],
                                ReservationDate = ((DateTime)reader["ReservationDay"]).ToShortDateString(),
                                StartDate = ((DateTime)reader["StartDate"]).ToShortDateString(),
                                EndDate = ((DateTime)reader["EndDate"]).ToShortDateString(),
                                CustomerName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}",
                                PhoneNumber = (string)reader["PhoneNumber"],
                                Email = (string)reader["Email"],
                                OfficeSpaceName = (string)reader["OfficeSpaceName"],
                                OfficeName = (string)reader["OfficeName"],
                                ReservationServices = new ObservableCollection<ServiceModel>()
                            };

                            officeReportModel.Period = $"{officeReportModel.StartDate} - {officeReportModel.EndDate}";

                            officeReportCollection.Add(officeReportModel);
                        }

                        return officeReportCollection;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches the additional services for each report in the given collection.
        /// </summary>
        /// <param name="officeReportsCollection">The collection of OfficeReportsModels to update.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task FetchOfficeReportServices(ObservableCollection<OfficeReportModel> officeReportsCollection)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT s.Name
                                           FROM ReservationService rs
                                           JOIN AdditionalService s ON s.ServiceID = rs.ServiceID
                                           WHERE ReservationID = @ReservationID;";

                foreach (var report in officeReportsCollection)
                {
                    using (var command = new MySqlCommand(STATEMENT, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationID", report.ReservationID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var serviceModel = new ServiceModel
                                {
                                    Name = (string)reader["Name"]
                                };

                                report.ReservationServices.Add(serviceModel);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fetches a collection of service reports for a specific office space within a specified time period.
        /// </summary>
        /// <param name="officeSpaceID">The ID of the office space to fetch service reports for.</param>
        /// <param name="startDate">The start date of the time period.</param>
        /// <param name="endDate">The end date of the time period.</param>
        /// <returns>An observable collection of service reports.</returns>
        public static async Task<ObservableCollection<ServiceReportModel>> FetchServiceReports(int officeSpaceID, DateTime startDate, DateTime endDate)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ohtu1"].ConnectionString))
            {
                await connection.OpenAsync();

                const string STATEMENT = @"SELECT r.ReservationID, r.StartDate, r.EndDate, s.Name,
                                                  s.Description, s.Price, os.Name AS OfficeSpaceName,
                                                  o.OfficeName, c.FirstName, c.LastName
                                           FROM Reservation r
                                           JOIN Customer c ON c.CustomerID = r.CustomerID
                                           JOIN ReservationService rs ON rs.ReservationID = r.ReservationID
                                           JOIN AdditionalService s ON s.ServiceID = rs.ServiceID
                                           JOIN OfficeSpace os ON os.SpaceID = s.SpaceID
                                           JOIN Office o ON o.OfficeID = os.OfficeID
                                           WHERE r.StartDate >= @StartDate AND r.EndDate <= @EndDate
                                           AND os.SpaceID = @SpaceID
                                           ORDER BY r.StartDate;";

                using (var command = new MySqlCommand(STATEMENT, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", officeSpaceID);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var serviceReportCollection = new ObservableCollection<ServiceReportModel>();

                        while (await reader.ReadAsync())
                        {
                            var serviceReportModel = new ServiceReportModel
                            {
                                ReservationID = (int)reader["ReservationID"],
                                StartDate = ((DateTime)reader["StartDate"]).ToShortDateString(),
                                EndDate = ((DateTime)reader["EndDate"]).ToShortDateString(),
                                CustomerName = $"{reader["FirstName"]} {reader["LastName"]}",
                                OfficeName = (string)reader["OfficeName"],
                                OfficeSpaceName = (string)reader["OfficeSpaceName"],
                                ServiceName = (string)reader["Name"],
                                ServiceDescription = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"],
                                ServicePrice = reader["Price"].ToString()
                            };

                            serviceReportModel.Period = $"{serviceReportModel.StartDate} - {serviceReportModel.EndDate}";

                            serviceReportCollection.Add(serviceReportModel);
                        }

                        return serviceReportCollection;
                    }
                }
            }
        }
    }
}