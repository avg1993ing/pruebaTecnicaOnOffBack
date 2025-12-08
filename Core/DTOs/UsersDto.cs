using Core.Entities;

namespace Core.DTOs
{
    public class UsersDto : BaseDto
    {
        public string NameUser { get; set; }
        public UsersDto IdUsersNavigation { get; set; }
    }
}
