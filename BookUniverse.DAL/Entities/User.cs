namespace BookUniverse.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BookUniverse.DAL.Constants.ValidationConstants;
    using BookUniverse.DAL.Enums;

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(UserValidationConstants.USERNAME_MIN_LENGTH)]
        [MaxLength(UserValidationConstants.USERNAME_MAX_LENGTH)]
        public string Username { get; set; }

        [Required]
        [MinLength(UserValidationConstants.EMAIL_MIN_LENGTH)]
        [MaxLength(UserValidationConstants.EMAIL_MAX_LENGTH)]
        public string Email { get; set; }

        [Required]
        [MaxLength(UserValidationConstants.PASSWORD_MAX_LENGTH)]
        public string Password { get; set; }

        [Required]
        public Roles Role { get; set; } = Roles.User;
    }
}
