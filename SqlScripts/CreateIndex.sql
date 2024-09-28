CREATE INDEX Office_name_index
ON Office(OfficeName);

CREATE INDEX Customer_firstname_index
ON Customer(Firstname);

CREATE INDEX Customer_lastname_index
ON Customer(Lastname);

CREATE INDEX Additional_service_index
ON AdditionalService(NAME);

CREATE INDEX OfficeSpace_name_index
ON OfficeSpace(NAME);

CREATE INDEX OfficeSpace_officeID_index
ON OfficeSpace(OfficeID);

CREATE INDEX Reservation_customerID_index
ON Reservation(CustomerID);

CREATE INDEX Invoice_customerID_index
ON Invoice(CustomerID);

CREATE INDEX Invoice_reservationID_index
ON Invoice(ReservationID);