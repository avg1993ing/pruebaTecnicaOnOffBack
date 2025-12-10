namespace Core.Entities
{
    public class TaskUser : BaseEntity
    {
        public int idUsers { get; set; }    
        public string NameTask { get; set; }
        public DateTime DateTask { get; set; }
        public bool Complete { get; set; }
        public virtual Users IdUsersNavigation { get; set; }
    }
}
