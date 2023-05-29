using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models;
using UserService.Repository;

namespace UserService.Api.JWTWebAuthentication
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly IConfiguration iconfiguration;
        private readonly IUserRepository _userRepository;
       
        public JWTManagerRepository(IConfiguration iconfiguration, IUserRepository userRepository)
        {
            this.iconfiguration = iconfiguration;
            _userRepository = userRepository;
        }

        public Tokens Authenticate(UserLogin user)
        {
            if (_userRepository.Authenticate(user).Result==null)
            {
                return null;
            }

            // Else we generate JSON Web Token
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Issuer= iconfiguration["JWT:Issuer"],
            //    Audience = iconfiguration["JWT:Audience"],
            //    Subject = new ClaimsIdentity(new Claim[]
            //  {
            //         new Claim(ClaimTypes.Name, user.Name)
            //  }),
            //    Expires = DateTime.UtcNow.AddMinutes(10),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return new Tokens { Token = tokenHandler.WriteToken(token) };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", Guid.NewGuid().ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience= iconfiguration["JWT:Audience"],
                Issuer= iconfiguration["JWT:Issuer"],
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };

        }

    }
}
