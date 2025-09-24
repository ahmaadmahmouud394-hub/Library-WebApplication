using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_WebApplication.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateOfTransaction { get; set; }
        [ForeignKey("Book_Id")]
        [Required]
        
        public int IdBook { get; set; }
        public Book Book { get; set; }
        [ForeignKey("User_Id")]
        [Required]
        public int IdUser { get; set; }
        public User User { get; set; }

    }
}
