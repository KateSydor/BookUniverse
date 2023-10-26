using System.ComponentModel.DataAnnotations;

namespace BookUniverse.BLL.DTOs
{
    public class RegistrationDto
    {
        public string? Username { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9.]{3,20}@(?:[a-zA-Z0-9]{2,20}\.){1,30}[a-zA-Z]{2,10}$", ErrorMessage = "Not valid email.")]
        public string? Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,15}$", ErrorMessage = "Not valid password.")]
        public string? Password { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,15}$", ErrorMessage = "Not valid password.")]
        public string? RepeatPassword { get; set; }
    }
}
