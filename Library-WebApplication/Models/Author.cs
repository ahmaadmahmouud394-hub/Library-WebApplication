using System.ComponentModel.DataAnnotations;

namespace Library_WebApplication.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime DateOfDeath { get; set; }
        public ICollection<Book> Books { get; set; }

    }
}
