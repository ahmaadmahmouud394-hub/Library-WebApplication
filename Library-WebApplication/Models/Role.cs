using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library_WebApplication.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
