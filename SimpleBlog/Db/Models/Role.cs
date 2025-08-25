namespace SimpleBlog.Db.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //Навигационное свойство
        public List<User> Users { get; set; } = new();
        
    }
}
