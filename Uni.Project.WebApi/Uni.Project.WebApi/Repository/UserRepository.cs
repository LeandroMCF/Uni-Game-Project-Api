using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Uni.Project.WebApi.Context;
using Uni.Project.WebApi.Domain;
using Uni.Project.WebApi.Interface;

namespace Uni.Project.WebApi.Repository
{
    public class UserRepository : IUserInterface
    {
        UniContext ctx = new UniContext();

        public string CadUser(UserDomain user)
        {
            try
            {
                UserDomain findUser = new UserDomain();
                findUser = ctx.Users.FirstOrDefault(x => x.Email == user.Email);

                if (findUser != null)
                {
                    return "userError";
                }
                else
                {
                    if (Convert.ToInt32(user.Status) > 1)
                    {
                        return "Error - Status not allowed";
                    }
                    else if (Convert.ToInt32(user.Permission) > 2)
                    {
                        return "Error - Permission not allowed";
                    }

                    if (user.Password.Count() < 7)
                    {
                        return "passError";
                    }

                    using (var sha256 = SHA256.Create())
                    {
                        // Converte a senha em um array de bytes
                        byte[] senhaBytes = Encoding.UTF8.GetBytes(user.Password);

                        // Calcula o hash da senha
                        byte[] hashBytes = sha256.ComputeHash(senhaBytes);

                        // Converte o hash em uma string hexadecimal
                        user.Password = BitConverter.ToString(hashBytes).Replace("-", "");
                    }

                    var getReturn = ctx.Users.Add(user).ToString();
                    ctx.SaveChanges();
                    return getReturn;
                }
            }
            catch (Exception)
            {

                return "serverError";
            }
        }

        public string Login(string email, string password)
        {
            string chaveSecreta = "UniToken";
            var chaveBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(chaveBytes);
            }
            var chave = new SymmetricSecurityKey(chaveBytes);

            using (var sha256 = SHA256.Create())
            {
                // Converte a senha em um array de bytes
                byte[] senhaBytes = Encoding.UTF8.GetBytes(password);

                // Calcula o hash da senha
                byte[] hashBytes = sha256.ComputeHash(senhaBytes);

                // Converte o hash em uma string hexadecimal
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "");

                try
                {
                    UserDomain findUser = new UserDomain();
                    findUser = ctx.Users.FirstOrDefault(x => x.Email == email && x.Password == hashString);

                    // Compara o hash da senha de entrada com o hash armazenado
                    if (findUser != null)
                    {
                        var token = GerarToken(findUser, chave);
                        return token;
                    }
                    else
                    {
                        return "userError";
                    }
                }
                catch (Exception)
                {

                    return "serverError";
                }
            }
        }

        public static string GerarToken(UserDomain user, SymmetricSecurityKey chave)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Permission", user.Permission.ToString()),
                new Claim("StatusEnum", user.Status.ToString()),
                new Claim("IdDownload", user.Download.ToString()),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool VerificarToken(SymmetricSecurityKey chave)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken("UniToken", new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = chave,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Download(string id)
        {
            UserDomain user = ctx.Users.FirstOrDefault(x => x.UserId.ToString() == id);
            if (user.Download == false)
            {
                user.Download = true;
                user.UpdateTime = DateTime.Now;
                ctx.Users.Update(user);
                ctx.SaveChanges();

                return "user-updated";
            }
            else
            {
                return "user-downloaded";
            }
        }
    }
}
