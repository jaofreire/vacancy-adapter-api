using CurriculumAdapter.API.Models.Enums;

namespace CurriculumAdapter.API.Models
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserTypeEnum Type { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash {  get; set; } = string.Empty;
    }
}
