using System.ComponentModel.DataAnnotations;

namespace Library_WebApplication.Models
{
    public class Tipology
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
