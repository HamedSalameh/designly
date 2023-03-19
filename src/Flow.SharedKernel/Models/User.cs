namespace Flow.SharedKernel.Models
{
    public class User
    {
        public string UserID { get; private set; }

        public string Username { get; private set; }
        public string AccessToken { get => _accessToken; set => _accessToken = value; }
        public int? ExpiresIn { get => _expiresIn; set => _expiresIn = value; }
        public string IdToken { get => _idToken; set => _idToken = value; }
        public string RefreshToken { get => _refreshToken; set => _refreshToken = value; }
        public string TokenType { get => _tokenType; set => _tokenType = value; }

        private string? _accessToken;

        private int? _expiresIn;

        private string? _idToken;

        private string? _refreshToken;

        private string? _tokenType;

        public User(string userID, string username)
        {
            UserID = userID;
            Username = username;
        }
    }
}
