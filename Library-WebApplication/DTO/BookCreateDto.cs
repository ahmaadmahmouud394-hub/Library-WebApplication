namespace Library_WebApplication.DTO
{
    public class BookCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int PubblisherId { get; set; }
        public int TipologyId { get; set; }
        public double Price { get; set; }
    }
}
