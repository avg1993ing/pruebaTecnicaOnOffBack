namespace Core.DTOs
{
    public class TaskUserDto : BaseDto
    {
        public int idUsers { get; set; }
        public string NameTask { get; set; }
        public DateTime DateTask { get; set; }
        public bool Complete { get; set; }
    }
}
