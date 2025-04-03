using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Infrastructure.Data.Queries
{
    public static class ContactQueries
    {
        public const string InsertContact = @"
        INSERT INTO Contacts (Name, LastName, Email, IPAddress, SubmissionTime)
        VALUES (@Name, @LastName, @Email, @IPAddress, @SubmissionTime);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public const string GetLastSubmissionTime = @"
        SELECT TOP 1 SubmissionTime
        FROM Contacts
        WHERE IPAddress = @IPAddress
        ORDER BY SubmissionTime DESC";

        public const string InsertContactDetails = @"
        INSERT INTO ContactDetails (
            ContactId, ContactDetailsName, UserName, ContactDetailsEmail, 
            Phone, WebSite, Street, Suite, City, ZipCode, 
            Lat, Lng, CompanyName, CatchPhrase, Bs)
        VALUES (
            @ContactId, @ContactDetailsName, @UserName, @ContactDetailsEmail, 
            @Phone, @WebSite, @Street, @Suite, @City, @ZipCode, 
            @Lat, @Lng, @CompanyName, @CatchPhrase, @Bs)";
    }
}
