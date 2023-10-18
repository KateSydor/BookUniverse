namespace BookUniverse.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Folder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FolderName { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
