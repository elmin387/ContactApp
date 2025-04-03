using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Application.Dto
{
    public class ContactDetailDto
    {
        public string? UserName { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }

        public AddressDto? Address { get; set; }
        public CompanyDto? Company { get; set; }
    }

    public class AddressDto
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public RegionDto? Geo { get; set; }
    }

    public class RegionDto
    {
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }

    public class CompanyDto
    {
        public string? Name { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }
    }
}
