using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlog.Db.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string? Text { get; set; }

        //Внешний ключ
        [ForeignKey ("User")]
        public int User_Id { get; set; }
        //Навигационное свойство
        public User User { get; set; }
        //Внешний ключ
        [ForeignKey("Post")]
        public int Post_Id { get; set; }
        //Навигационное свойство
        public Post Post { get; set; }
    }
}
