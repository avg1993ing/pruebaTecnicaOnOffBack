namespace Core.Entities
{
    public class Users : BaseEntity
    {
        public string NameUser { get; set; }
        public string PasswordUser { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public ICollection<TaskUser> TaskUsers { get; set; }    
    }
}
