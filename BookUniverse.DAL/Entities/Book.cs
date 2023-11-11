namespace BookUniverse.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(BookValidationConstants.BOOKTITLE_MIN_LENGTH)]
        [MaxLength(BookValidationConstants.BOOKTITLE_MAX_LENGTH)]
        public string Title { get; set; }

        [Required]
        [MinLength(BookValidationConstants.AUTHOR_MIN_LENGTH)]
        [MaxLength(BookValidationConstants.AUTHOR_MAX_LENGTH)]
        public string Author { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        [MinLength(BookValidationConstants.DESCRIPTION_MIN_LENGTH)]
        [MaxLength(BookValidationConstants.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [Range(1, 1200)]
        public int NumberOfPages { get; set; }

        [Required]
        [Range(0.0, 5.0)]
        public double Rating { get; set; } = 0.0;

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        // public ICollection<UserBook> UserBooks { get; set; }
    }
}
