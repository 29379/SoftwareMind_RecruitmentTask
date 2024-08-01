-- Seed Roles
INSERT INTO Roles (RoleName) VALUES ('ADMIN');
INSERT INTO Roles (RoleName) VALUES ('EMPLOYEE');

-- Seed BookingStatuses
INSERT INTO BookingStatuses (StatusName) VALUES ('BOOKED');
INSERT INTO BookingStatuses (StatusName) VALUES ('CANCELED');
INSERT INTO BookingStatuses (StatusName) VALUES ('COMPLETED');
INSERT INTO BookingStatuses (StatusName) VALUES ('REJECTED');

-- Seed Offices
INSERT INTO Offices (Name, City, TotalFloors, TotalDesks) VALUES ('Main Office', 'Wrocław', 2, 16);
INSERT INTO Offices (Name, City, TotalFloors, TotalDesks) VALUES ('Second Office', 'Kraków', 1, 7);


-- Seed OfficeFloors
INSERT INTO Floors (OfficeId, FloorNumber, NumberOfDesks) VALUES (1, 1, 8);
INSERT INTO Floors (OfficeId, FloorNumber, NumberOfDesks) VALUES (1, 2, 8);
INSERT INTO Floors (OfficeId, FloorNumber, NumberOfDesks) VALUES (2, 1, 7);


-- Seed Desks
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);
INSERT INTO Desks (OfficeFloorId) VALUES (1);

INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);
INSERT INTO Desks (OfficeFloorId) VALUES (2);

INSERT INTO Desks (OfficeFloorId) VALUES (3);
INSERT INTO Desks (OfficeFloorId) VALUES (3);
INSERT INTO Desks (OfficeFloorId) VALUES (3);
INSERT INTO Desks (OfficeFloorId) VALUES (3);
INSERT INTO Desks (OfficeFloorId) VALUES (3);
INSERT INTO Desks (OfficeFloorId) VALUES (3);
INSERT INTO Desks (OfficeFloorId) VALUES (3);


-- Seed AppUsers
INSERT INTO Users (Email, FirstName, LastName, HashPassword) VALUES ('john.doe@example.com', 'John', 'Doe', 'pswd123');
INSERT INTO Users (Email, FirstName, LastName, HashPassword) VALUES ('jane.smith@example.com', 'Jane', 'Smith', 'pswd1234');

-- Seed UserRoles
INSERT INTO UserRoles (UserEmail, RoleName) VALUES ('john.doe@example.com', 'ADMIN');
INSERT INTO UserRoles (UserEmail, RoleName) VALUES ('jane.smith@example.com', 'EMPLOYEE');

-- Seed Bookings
INSERT INTO Bookings (UserEmail, DeskId, BookingStatusName, startTime, endTime) VALUES 
('john.doe@example.com', 1, 'BOOKED', '2024-08-01T09:00:00', '2024-08-01T17:00:00'),
('jane.smith@example.com', 2, 'ANNULED', '2024-08-02T10:00:00', '2024-08-02T15:00:00');
