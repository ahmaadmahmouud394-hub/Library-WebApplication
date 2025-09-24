using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_WebApplication.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Author_Id")]
        [Required]
        public int IdAuthor { get; set; }
        public Author Author { get; set; }

        [ForeignKey("Pubblisher_Id")]
        [Required]
        public int IdPubblisher { get; set; }
        public Pubblisher Pubblisher { get; set; }

        [ForeignKey("Tipology_Id")]
        [Required]
        public int IdTipology { get; set; }
        public Tipology Tipology { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime PublishingDate { get; set; }
        [Required]
        public double Price { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}
