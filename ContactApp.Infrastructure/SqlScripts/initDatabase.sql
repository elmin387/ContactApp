CREATE DATABASE ContactAppDB;
GO
USE ContactAppDB;
GO

-- Contacts table
CREATE TABLE Contacts (
    ContactId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    IPAddress NVARCHAR(50) NOT NULL,
    SubmissionTime DATETIME2 NOT NULL
);

-- ContactDetails table
CREATE TABLE ContactDetails (
    ContactDetailsId INT IDENTITY(1,1) PRIMARY KEY,
    ContactDetailsName NVARCHAR(100) NULL,
    UserName NVARCHAR(100) NULL,
    ContactDetailsEmail NVARCHAR(255) NULL,
    Phone NVARCHAR(50) NULL,
    WebSite NVARCHAR(255) NULL,
    ContactId INT NOT NULL,
    CONSTRAINT FK_ContactDetails_Contacts FOREIGN KEY (ContactId) REFERENCES Contacts(ContactId)
);

-- Address table
CREATE TABLE Address (
    AddressId INT IDENTITY(1,1) PRIMARY KEY,
    ContactDetailsId INT NOT NULL,
    Street NVARCHAR(255) NULL,
    Suite NVARCHAR(100) NULL,
    City NVARCHAR(100) NULL,
    ZipCode NVARCHAR(20) NULL,
    CONSTRAINT FK_Address_ContactDetails FOREIGN KEY (ContactDetailsId) REFERENCES ContactDetails(ContactDetailsId)
);

-- Region (Geo) table
CREATE TABLE Region (
    RegionId INT IDENTITY(1,1) PRIMARY KEY,
    AddressId INT NOT NULL,
    Lat NVARCHAR(50) NULL,
    Lng NVARCHAR(50) NULL,
    CONSTRAINT FK_Region_Address FOREIGN KEY (AddressId) REFERENCES Address(AddressId)
);

-- Company table
CREATE TABLE Company (
    CompanyId INT IDENTITY(1,1) PRIMARY KEY,
    ContactDetailsId INT NOT NULL,
    CompanyName NVARCHAR(255) NULL,
    CatchPhrase NVARCHAR(255) NULL,
    Bs NVARCHAR(255) NULL,
    CONSTRAINT FK_Company_ContactDetails FOREIGN KEY (ContactDetailsId) REFERENCES ContactDetails(ContactDetailsId)
);

-- Create indexes for performance
CREATE INDEX IX_Contacts_IPAddress_SubmissionTime ON Contacts(IPAddress, SubmissionTime);
CREATE INDEX IX_ContactDetails_ContactId ON ContactDetails(ContactId);
CREATE INDEX IX_Address_ContactDetailsId ON Address(ContactDetailsId);
CREATE INDEX IX_Region_AddressId ON Region(AddressId);
CREATE INDEX IX_Company_ContactDetailsId ON Company(ContactDetailsId);