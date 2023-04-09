using System.Text;

namespace Flow.IdentityService
{
    public class AWSCognitoConfiguration
    {
        public string ClientId { get; set; }
        public string PoolId { get; set; }

        public override string ToString()
        {
            var encodedClientId = EncodeToBase64(ClientId);
            var encodedPoolId = EncodeToBase64(PoolId);

            return $"{nameof(PoolId)} : {encodedPoolId}, {nameof(ClientId)} : {encodedClientId}";
        }

        private static string EncodeToBase64(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }
    }
}
