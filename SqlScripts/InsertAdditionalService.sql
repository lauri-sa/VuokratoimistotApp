/*Inserts for offered additional services for each officespace.*/

INSERT INTO AdditionalService (NAME, Price, SpaceID)
VALUES ('Kahvitarjoilu', 8.90, 1);

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Siivous', 12.50, 1, 'Tarjoamme lisäpalveluna toimistotilojen siivouksen hintaan 20.50e/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Videotykki', 25.90, 1, 'Vuokraamme lisäpalveluina videotykin hintaan 25.90e/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID)
VALUES ('Kahvitarjoilu', 8.90, 3);

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Siivous', 12.50, 3, 'Tarjoamme lisäpalveluna toimistotilojen siivouksen hintaan 20.50e/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Videotykki', 25.90, 3, 'Vuokraamme lisäpalveluina videotykin hintaan 25.90/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID)
VALUES ('Kahvitarjoilu', 8.90, 4);

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Tulostin', 10.00, 4, 'Vuokraamme lisäpalveluina tulostimen hintaan 10.00e/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID)
VALUES ('Lounas', 12.90, 5);

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Tulostin', 10.00, 5, 'Vuokraamme lisäpalveluina tulostimen hintaan 10.00e/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Kannettava tietokone', 18.90, 6, 'Vuokraamme lisäpalveluina läppärin hintaan 18.90/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Tulostin', 10.00, 7, 'Vuokraamme lisäpalveluina tulostimen hintaan 10.00e/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID)
VALUES ('Lounas', 12.90, 8);

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Videotykki', 25.90, 8, 'Vuokraamme lisäpalveluina videotykin hintaan 25.90/päivä.');

INSERT INTO AdditionalService (NAME, Price, SpaceID, Description)
VALUES ('Kannettava tietokone', 18.90, 8, 'Vuokraamme lisäpalveluina läppärin hintaan 18.90/päivä.');












 /* Description VARCHAR(250),
  Price FLOAT NOT NULL,
  ServiceID INT AUTO_INCREMENT NOT NULL,
  Name VARCHAR(45) NOT NULL,
  SpaceID INT NOT NULL,
  PRIMARY KEY (ServiceID),
  FOREIGN KEY (SpaceID) REFERENCES OfficeSpace(SpaceID) ON DELETE CASCADE*/