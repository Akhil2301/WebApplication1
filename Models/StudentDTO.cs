using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }

       
        [Required(ErrorMessage ="Student name is required")]
        [StringLength(30)]
        public string StudentName { get; set; }

        [EmailAddress(ErrorMessage ="Please enter a valid email address")]        
        public string Email { get; set; }

       
        [Range(10,20)]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string confirmpassword { get; set; }

        [DateCheck]
        public DateTime AdmissionDate { get; set; }
    }
}
