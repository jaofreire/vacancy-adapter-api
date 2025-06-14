using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Models.Enums;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using CurriculumAdapter.API.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CurriculumAdapter.API.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<APIResponse<string>> Login(LoginInputDTO loginInput)
        {
            var existsUserByEmail = await _userRepository.Get(x => x.Email == loginInput.Email);

            if (existsUserByEmail.Any() is false)
                return new APIResponse<string>(false, 404, "Usuário não encontrado");

            var user = existsUserByEmail.First();

            if (!PasswordHashUtils.VerifyPassword(loginInput.Password, user.PasswordHash))
                return new APIResponse<string>(false, 400, "Senha incorreta");

            var token = GenerateToken(user);

            return new APIResponse<string>(true, 200, "Usuário autenticado com sucesso!", token, null);
        }

        private string GenerateToken(UserModel model)
        {
            string secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? _configuration["JWT:Secret"]!;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var claims = CreateClaims(model.Id, model.Type);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                //Adicionar issue e audience

                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity CreateClaims(Guid id, UserTypeEnum type)
        {
            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, type.ToString()));

            return claimsIdentity;
        }
    }
}
