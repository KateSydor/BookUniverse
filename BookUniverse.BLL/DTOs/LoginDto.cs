using System.ComponentModel.DataAnnotations;

namespace BookUniverse.BLL.DTOs
{
    public class LoginDto
    {
        public string? Username { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,15}$", ErrorMessage = "Not valid password.")]
        public string? Password { get; set; }
    }
}
