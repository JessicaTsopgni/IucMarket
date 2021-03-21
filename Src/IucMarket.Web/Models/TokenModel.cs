using IucMarket.Common;

namespace IucMarket.Web.Models
{
    public class TokenModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleOptions Role { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public TokenModel()
        {

        }

        public TokenModel(string name, string email, RoleOptions role, string token, int expiresIn)
        {
            Name = name;
            Email = email;
            Role = role;
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
