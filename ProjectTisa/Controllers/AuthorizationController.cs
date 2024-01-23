using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectPop.Controllers;
using ProjectTisa.Controllers.GeneralData;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using ProjectTisa.Models.Requests;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectTisa.Controllers
{
    /// <summary>
    /// Controller for Authorize at server. Using JWT Token as result.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(ILogger<WeatherForecastController> logger, MainDbContext context, IOptions<RouteConfig> config) : ControllerBase
    {
        private readonly RouteConfig _routeConfig = config.Value;
        private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;
        /// <summary>
        /// Method to reveice JWT Token by passing 
        /// </summary>
        /// <param name="user">User that exist in database.</param>
        /// <returns>200: String represent JWT Token.</returns>
        [HttpPost("Authorize")]
        public ActionResult<string> Authorize(string username, string password)
        {
            User? user = context.Users.FirstOrDefault(x => x.Username == username);
            if (user == null || !VerifyPassword(password, user.PasswordHash, Convert.FromHexString(user.Salt)))
            {
                return BadRequest(ResAnswers.BadRequest);
            }
            return Ok(CreateToken(user));
        }
        /// <summary>
        /// Check is email exist in <b>User</b> table. 
        /// </summary>
        /// <param name="email">Email to check.</param>
        /// <returns>200: Result of check: <c>true</c> - email exist, <c>false</c> - email doesn't exist. </returns>
        [HttpGet("CheckEmail")]
        public ActionResult<bool> CheckEmail(string email)
        {
            return Ok(IsEmailExist(email));
        }
        //
        [HttpGet("CheckUsername")]
        public ActionResult<bool> CheckUsername(string username)
        {
            return Ok(IsUsernameExist(username));
        }
        [Authorize]
        [HttpGet("GetUser")]
        public ActionResult<bool> GetUser()
        {
            User? user;
            try
            {
                string currentUsername = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value ?? throw new NullReferenceException("Username doesn't exist in JWT token.");
                user = context.Users.FirstOrDefault(x => x.Username == currentUsername) ?? throw new NullReferenceException("User doesn't exist in database.");
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
           
            return Ok(user);
        }
        /// <summary>
        /// Registrate new user at database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>200: String represent JWT Token.</returns>
        [HttpPost("Registrate")]
        public async Task<ActionResult<string>> Registrate([FromBody]UserCreation userCreation)
        {
            User user = new() { Username = userCreation.Username, Email = userCreation.Email, Salt = string.Empty, PasswordHash = string.Empty };
            ValidationContext valContext = new(user);
            List<ValidationResult> valResults = [];
            //check is exist?
            //check smtp email?
            if (!Validator.TryValidateObject(user, valContext, valResults, true))
            {
                return BadRequest(valResults);
            }

            byte[] tempSalt = CreateSalt(_routeConfig.AuthData.SaltSize);
            user.RegistrationDate = DateTime.UtcNow;
            user.Salt = Convert.ToHexString(tempSalt);
            user.PasswordHash = HashPasword(userCreation.Password, tempSalt);
            context.Add(user);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, user);
            return Ok(CreateToken(user));
        }
        //
        private string CreateToken(User user)
        {
            SymmetricSecurityKey symKey = new(Encoding.UTF8.GetBytes(_routeConfig.AuthData.IssuerSigningKey));
            SigningCredentials credentials = new(symKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                issuer: _routeConfig.AuthData.Issuer, audience: _routeConfig.AuthData.Audience,
                claims: [new Claim(nameof(user.Username), user.Username), new Claim(nameof(user.PasswordHash), user.PasswordHash)],
                expires: DateTime.Now.Add(_routeConfig.AuthData.ExpirationTime), signingCredentials: credentials);
            string rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            LogMessageCreator.TokenGranted(logger, user.Username);
            return rawToken;
        }
        //
        private static byte[] CreateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }
        //
        private string HashPasword(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _routeConfig.AuthData.IterationCount,
                _hashAlgorithm,
                _routeConfig.AuthData.SaltSize);
            return Convert.ToHexString(hash);
        }
        //
        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _routeConfig.AuthData.IterationCount, _hashAlgorithm, _routeConfig.AuthData.SaltSize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
        //
        private bool IsEmailExist(string email)
        {
            return context.Users.Any(u => u.Email.ToLower() == email.ToLower());
        }
        //
        private bool IsUsernameExist(string username)
        {
            return context.Users.Any(u => u.Username.ToLower() == username.ToLower());
        }
    }
}
