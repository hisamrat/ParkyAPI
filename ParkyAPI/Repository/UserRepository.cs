using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appsettings;

        public UserRepository(ApplicationDbContext applicationDbContext,
            IOptions<AppSettings> appsettings
            
            )
        {
            _applicationDbContext = applicationDbContext;
            _appsettings = appsettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            var user = _applicationDbContext.Users.SingleOrDefault(x => x.Username
              == username && x.Password == password);
            if(user==null)
            {
                return null;
            }
            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Expires=DateTime.UtcNow.AddDays(7),
                SigningCredentials=new SigningCredentials
                (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            throw new NotImplementedException();
        }

        public User Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
