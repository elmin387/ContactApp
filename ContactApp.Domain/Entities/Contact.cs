﻿
namespace ContactApp.Domain.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public string SubmissionTime { get; set; }
    }
}
