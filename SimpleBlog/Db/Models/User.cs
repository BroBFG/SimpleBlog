using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlog.Db.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? Password { get; set; }
        //Навигационное свойство
        public List<Comment> Comments { get; set; } = new();

        //Внешний ключ
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
        //Навигационное свойство
        public Role Role { get; set; }
    }
}
