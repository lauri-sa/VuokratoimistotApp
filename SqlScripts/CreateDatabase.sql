-- Creates database for the project
CREATE DATABASE Ohtu1_2023_R04;

-- Creates Office -parent table
CREATE TABLE Office
(
  OfficeID INT AUTO_INCREMENT NOT NULL,
  OfficeName VARCHAR(45) NOT NULL,
  StreetAddress VARCHAR(45) NOT NULL,
  City VARCHAR(15) NOT NULL,
  Postalcode VARCHAR(15) NOT NULL,
  PhoneNumber VARCHAR(15) NOT NULL,
  Email VARCHAR(45) NOT NULL,
  PRIMARY KEY (OfficeID)
);

-- Creates Customer -parent table
CREATE TABLE Customer
(
  CustomerID INT AUTO_INCREMENT NOT NULL,
  Firstname VARCHAR(25) NOT NULL,
  Lastname VARCHAR(25) NOT NULL,
  StreetAddress VARCHAR(45) NOT NULL,
  Postalcode VARCHAR(15) NOT NULL,
  City VARCHAR(15) NOT NULL,
  PhoneNumber VARCHAR(15) NOT NULL,
  Email VARCHAR(45) NOT NULL,
  PRIMARY KEY (CustomerID)
);

-- Creates OfficeSpace -child table, which is connected to Office -table
CREATE TABLE OfficeSpace
(
  SpaceID INT AUTO_INCREMENT NOT NULL,
  OfficeID INT NOT NULL,
  Name VARCHAR(45) NOT NULL,
  Price FLOAT NOT NULL,
  Capacity INT NOT NULL,
  Size INT NOT NULL,
  PRIMARY KEY (SpaceID),
  FOREIGN KEY (OfficeID) REFERENCES Office(OfficeID) ON DELETE CASCADE
);

-- Creates Reservation -child table, which is connected to both Customer and OfficeSpace -tables
CREATE TABLE Reservation
(
  ReservationID INT AUTO_INCREMENT NOT NULL,
  CustomerID INT NOT NULL,
  StartDate DATE NOT NULL,
  EndDate DATE NOT NULL,
  ReservationDay DATE NOT NULL,
  PRIMARY KEY (ReservationID),
  FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE
);

-- Creates Invoice -child table, which is connected to both Customer and Reservation -tables
CREATE TABLE Invoice
(
  InvoiceID INT AUTO_INCREMENT NOT NULL,
  CustomerID INT NOT NULL,
  ReservationID INT NOT NULL,
  Date DATE NOT NULL,
  DueDate DATE NOT NULL,
  TotalSum FLOAT NOT NULL,
  PRIMARY KEY (InvoiceID),
  FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE,
  FOREIGN KEY (ReservationID) REFERENCES Reservation(ReservationID) ON DELETE CASCADE
);

-- Creates AdditionalService -child table, which is connected to OfficeSpace -table
CREATE TABLE AdditionalService
(
  ServiceID INT AUTO_INCREMENT NOT NULL,
  SpaceID INT NOT NULL,
  Name VARCHAR(45) NOT NULL,
  Description VARCHAR(250),
  Price FLOAT NOT NULL,
  PRIMARY KEY (ServiceID),
  FOREIGN KEY (SpaceID) REFERENCES OfficeSpace(SpaceID) ON DELETE CASCADE
);

-- Creates ReservationService -child table, which is connected to both AdditionalService- and Reservation -table.
CREATE TABLE ReservationService
(
  ServiceID INT NOT NULL,
  ReservationID INT NOT NULL,
  Price FLOAT NOT NULL,
  PRIMARY KEY (ServiceID, ReservationID),
  FOREIGN KEY (ServiceID) REFERENCES AdditionalService(ServiceID) ON DELETE CASCADE,
  FOREIGN KEY (ReservationID) REFERENCES Reservation(ReservationID) ON DELETE CASCADE
);

-- Creates ReservationOffice -child table, which is connected to both Reservation- and OfficeSpace -table.
CREATE TABLE ReservationOffice
(
  Price FLOAT NOT NULL,
  ReservationID INT NOT NULL,
  SpaceID INT NOT NULL,
  PRIMARY KEY (SpaceID, ReservationID),
  FOREIGN KEY (ReservationID) REFERENCES Reservation(ReservationID) ON DELETE CASCADE,
  FOREIGN KEY (SpaceID) REFERENCES OfficeSpace(SpaceID) ON DELETE CASCADE
);