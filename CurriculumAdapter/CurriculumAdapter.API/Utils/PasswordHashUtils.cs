namespace CurriculumAdapter.API.Utils
{
    public class PasswordHashUtils
    {
        private const int _workFactor = 12;

        public static string CreateHashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, _workFactor);

        public static bool VerifyPassword(string submitedPassword, string hashPassword)
            => BCrypt.Net.BCrypt.Verify(submitedPassword, hashPassword);
    }
}
