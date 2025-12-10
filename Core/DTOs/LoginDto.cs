namespace Core.DTOs
{
    public record LoginDto
    {
        public string NameUser { get; set; }
        public string PasswordUser { get; set; }
    }
}
