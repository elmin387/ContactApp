using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Domain.Entities
{
    public class ContactDetails
    {
        public int ContactDetailsId { get; set; }
        public string? UserName { get; set; }
        public Address? Address { get; set; }
        public string? Phone { get; set; }
        public string? WebSite { get; set; }
        public Company? Company { get; set; }
        public Contact? UserMainInfo {  get; set; }
    }

    public class Company
    {
        public string? CompanyName { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }
    }

    public class Address
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public Region? Geo { get; set; }
    }

    public class Region
    {
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }
}
