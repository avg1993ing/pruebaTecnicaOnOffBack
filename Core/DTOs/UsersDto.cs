using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DTOs
{
    public class UsersDto : BaseDto
    {
        public string NameUser { get; set; }
        public string Email { get; set; }
        [NotMapped]
        public string PasswordUser { get; set; }
    }
}
