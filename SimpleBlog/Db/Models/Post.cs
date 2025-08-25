using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Db.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }  
        public string? Text { get; set; }
        public DateOnly Date { get; set; }
        //Навигационное свойство
        public List<Comment> Comments { get; set; } = new();
    }
}
