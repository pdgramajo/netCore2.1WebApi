using System;

namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkEmail { get; set; }
        public string PersonTypes { get; set; }
        public int? EmploymentType { get; set; }
        public DateTime? DateOfHire { get; set; }
        public DateTime? FirstHireDate { get; set; }
        public DateTime? FirstEndOfContract { get; set; }
        public DateTime? EndOfContract { get; set; }
        public bool AllowLogin { get; set; }
    }
}
