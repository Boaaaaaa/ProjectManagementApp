using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please employee number.")]
        [RegularExpression(@"[0-9]{2}-[0-9]{4}-[a-zA-Z]{2}$", ErrorMessage = "The employee number's format is like \'12-3456-AB\'")]
        public string? EmployeeNumber { get; set; }

        [Required(ErrorMessage = "Please enter your first name.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        public string? LastName { get; set; }

        public string? FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }

        // FK:
        public int? ProjectId { get; set; }

        // And nav prop:
        public Project? Project { get; set; }
    }
}
