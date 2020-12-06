namespace WorkTimeSheet
{
    public class PasswordProtector
    {
        public PasswordProtector(string password, string salt, string hashedPassword)
        {
            Password = password;
            Salt = salt;
            HashedPassword = hashedPassword;
        }

        public string Password { get; }
        public string Salt { get; }
        public string HashedPassword { get; }

        public static PasswordProtector Create(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return new PasswordProtector(password, salt, hashedPassword);
        }
    }
}
