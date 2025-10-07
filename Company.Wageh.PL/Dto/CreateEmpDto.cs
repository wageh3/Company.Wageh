using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Wageh.PL.Dto
{
    public class CreateEmpDto
    {
        [Required(ErrorMessage ="Name Is Required !!")]
        public string Name { get; set; }
        [Range(18,60 , ErrorMessage ="Age Must Be Between 18 and 60")]
        public int Age { get; set; }
        [DataType(DataType.EmailAddress , ErrorMessage ="Email is Not Valid !!")]
        public string Email { get; set; }
        [RegularExpression(pattern: @"^[6-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime CreateAt { get; set; }
    }
}
