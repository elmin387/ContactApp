
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ContactAppDB')
BEGIN
    CREATE DATABASE ContactAppDB;
END
GO

USE ContactAppDB;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contacts]') AND type in (N'U'))
BEGIN
    CREATE TABLE Contacts (
        ContactId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(255) NOT NULL,
        IPAddress NVARCHAR(50) NOT NULL,
        SubmissionTime DATETIME2 NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactDetails]') AND type in (N'U'))
BEGIN
    CREATE TABLE ContactDetails (
        ContactDetailsId INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(100) NULL,
        Phone NVARCHAR(50) NULL,
        WebSite NVARCHAR(255) NULL,
        ContactId INT NOT NULL,
        CONSTRAINT FK_ContactDetails_Contacts FOREIGN KEY (ContactId) REFERENCES Contacts(ContactId)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Address]') AND type in (N'U'))
BEGIN
    CREATE TABLE Address (
        AddressId INT IDENTITY(1,1) PRIMARY KEY,
        ContactDetailsId INT NOT NULL,
        Street NVARCHAR(255) NULL,
        Suite NVARCHAR(100) NULL,
        City NVARCHAR(100) NULL,
        ZipCode NVARCHAR(20) NULL,
        CONSTRAINT FK_Address_ContactDetails FOREIGN KEY (ContactDetailsId) REFERENCES ContactDetails(ContactDetailsId)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Region]') AND type in (N'U'))
BEGIN
    CREATE TABLE Region (
        RegionId INT IDENTITY(1,1) PRIMARY KEY,
        AddressId INT NOT NULL,
        Lat NVARCHAR(50) NULL,
        Lng NVARCHAR(50) NULL,
        CONSTRAINT FK_Region_Address FOREIGN KEY (AddressId) REFERENCES Address(AddressId)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Company]') AND type in (N'U'))
BEGIN
    CREATE TABLE Company (
        CompanyId INT IDENTITY(1,1) PRIMARY KEY,
        ContactDetailsId INT NOT NULL,
        CompanyName NVARCHAR(255) NULL,
        CatchPhrase NVARCHAR(255) NULL,
        Bs NVARCHAR(255) NULL,
        CONSTRAINT FK_Company_ContactDetails FOREIGN KEY (ContactDetailsId) REFERENCES ContactDetails(ContactDetailsId)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Contacts_IPAddress_SubmissionTime' AND object_id = OBJECT_ID('Contacts'))
BEGIN
    CREATE INDEX IX_Contacts_IPAddress_SubmissionTime ON Contacts(IPAddress, SubmissionTime);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ContactDetails_ContactId' AND object_id = OBJECT_ID('ContactDetails'))
BEGIN
    CREATE INDEX IX_ContactDetails_ContactId ON ContactDetails(ContactId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Address_ContactDetailsId' AND object_id = OBJECT_ID('Address'))
BEGIN
    CREATE INDEX IX_Address_ContactDetailsId ON Address(ContactDetailsId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Region_AddressId' AND object_id = OBJECT_ID('Region'))
BEGIN
    CREATE INDEX IX_Region_AddressId ON Region(AddressId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Company_ContactDetailsId' AND object_id = OBJECT_ID('Company'))
BEGIN
    CREATE INDEX IX_Company_ContactDetailsId ON Company(ContactDetailsId);
END
GO