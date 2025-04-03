using ContactApp.Application.Interfaces.Repositories;
using ContactApp.Domain.Entities;
using ContactApp.Infrastructure.Data.Queries;
using Dapper;

namespace ContactApp.Infrastructure.Data.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public ContactRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<int> CreateContactAsync(Contact contact)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<int>(ContactQueries.InsertContact, new
            {
                contact.Name,
                contact.LastName,
                contact.Email,
                contact.IPAddress,
                SubmissionTime = DateTime.UtcNow,
            });
        }
        public async Task<DateTime> GetLastSubmissionTimeAsync(string ipAddress)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QuerySingleOrDefaultAsync<DateTime?>(
                ContactQueries.GetLastSubmissionTime,
                new { IPAddress = ipAddress });

            return result ?? DateTime.MinValue;
        }

        public async Task AddContactDetailsAsync(int contactId, ContactDetails contactDetails)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
          
                var contactDetailsQuery = @"
            INSERT INTO ContactDetails (
                ContactId, UserName, 
                Phone, WebSite)
            VALUES (
                @ContactId, @UserName, 
                @Phone, @WebSite);
            SELECT CAST(SCOPE_IDENTITY() as int)";

                int contactDetailsId = await connection.QuerySingleAsync<int>(contactDetailsQuery, new
                {
                    ContactId = contactId,
                    contactDetails.UserName,
                    contactDetails.Phone,
                    contactDetails.WebSite
                }, transaction);

                
                if (contactDetails.Address != null)
                {
                    var addressQuery = @"
                INSERT INTO Address (
                    ContactDetailsId, Street, Suite, City, ZipCode)
                VALUES (
                    @ContactDetailsId, @Street, @Suite, @City, @ZipCode);
                SELECT CAST(SCOPE_IDENTITY() as int)";

                    int addressId = await connection.QuerySingleAsync<int>(addressQuery, new
                    {
                        ContactDetailsId = contactDetailsId,
                        contactDetails.Address.Street,
                        contactDetails.Address.Suite,
                        contactDetails.Address.City,
                        contactDetails.Address.ZipCode
                    }, transaction);

                    
                    if (contactDetails.Address.Geo != null)
                    {
                        var geoQuery = @"
                    INSERT INTO Region (
                        AddressId, Lat, Lng)
                    VALUES (
                        @AddressId, @Lat, @Lng)";

                        await connection.ExecuteAsync(geoQuery, new
                        {
                            AddressId = addressId,
                            contactDetails.Address.Geo.Lat,
                            contactDetails.Address.Geo.Lng
                        }, transaction);
                    }
                }

               
                if (contactDetails.Company != null)
                {
                    var companyQuery = @"
                INSERT INTO Company (
                    ContactDetailsId, CompanyName, CatchPhrase, Bs)
                VALUES (
                    @ContactDetailsId, @CompanyName, @CatchPhrase, @Bs)";

                    await connection.ExecuteAsync(companyQuery, new
                    {
                        ContactDetailsId = contactDetailsId,
                        contactDetails.Company.CompanyName,
                        contactDetails.Company.CatchPhrase,
                        contactDetails.Company.Bs
                    }, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
