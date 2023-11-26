namespace BookUniverse.DAL.Entities
{
    using BookUniverse.DAL.Constants.ValidationConstants;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(CategoryValidationConstants.CATEGORYNAME_MIN_LENGTH)]
        [MaxLength(CategoryValidationConstants.CATEGORYNAME_MAX_LENGTH)]
        public string CategoryName { get; set; }
    }
}
