namespace BookUniverse.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int NumberOfPages { get; set; }

        [Required]
        public double Rating { get; set; } = 0.0;

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

		public ICollection<UserBook> UserBooks { get; set; }
	}
}
