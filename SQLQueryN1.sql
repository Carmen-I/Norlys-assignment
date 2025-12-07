CREATE DATABASE NorlysEmployers;
GO
USE NorlysEmployers;
GO


CREATE TABLE Office (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    MaxCapacity INT NOT NULL CHECK (MaxCapacity > 0)
);
GO

CREATE TABLE Person (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    OfficeId INT NOT NULL,

    CONSTRAINT FK_Person_Office FOREIGN KEY (OfficeId)
        REFERENCES Office(Id)
        ON DELETE NO ACTION     -- no cascade delete
        
);
GO

INSERT INTO Office (Name, MaxCapacity)
VALUES
( 'Aalborg', 6),
( 'Aarhus', 4),
('Copenhagen', 10);
GO


INSERT INTO Person (Name, LastName, BirthDate, OfficeId)
VALUES
('Anna', 'Jensen', '1998-05-12', 1),
('Mikkel', 'Olsen', '1994-11-23', 1),
('Sara', 'Larsen', '2001-02-02', 1);

-- Aarhus Office (Id=2)
INSERT INTO Person (Name, LastName, BirthDate, OfficeId)
VALUES
('Jonas', 'Hansen', '1997-09-14', 2),
('Laura', 'Nielsen', '1999-06-30', 2);

-- Copenhagen Office (Id=3)
INSERT INTO Person (Name, LastName, BirthDate, OfficeId)
VALUES
('Emil', 'Christensen', '1990-01-20', 3),
('Frederikke', 'Poulsen', '1995-08-02', 3),
('Tobias', 'Madsen', '2000-12-11', 3),
('Maria', 'Thomsen', '1996-03-28', 3);
GO

CREATE VIEW PersonWithOffice AS
SELECT
    p.Id AS PersonId,
    p.Name AS FirstName,
    p.LastName AS LastName,
    p.BirthDate AS BirthDate,
    o.Id AS OfficeId,
    o.Name AS OfficeName,
	o.MaxCapacity AS MaxOccupancy

FROM Person p
JOIN Office o ON p.OfficeId = o.Id;
 GO