using System.ComponentModel.DataAnnotations;

namespace Company.Wageh.PL.Dto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password Is Required !!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password Is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not match the Password")]
        public string ConfirmPassword { get; set; }
    }
}
