using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectPop.Controllers;
using ProjectTisa.Controllers.GeneralData;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers
{
    /// <summary>
    /// Controller for auth at server. Using Bearer JWT Token as authorize method.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILogger<WeatherForecastController> logger, MainDbContext context, IOptions<RouteConfig> config) : ControllerBase
    {
        private readonly AuthData _authData = config.Value.AuthData;
        /// <summary>
        /// Reveice JWT Token by passing username and password.
        /// </summary>
        /// <param name="user">User that exist in database.</param>
        /// <returns>200: String represent JWT Token.</returns>
        [HttpPost("Authorize")]
        public ActionResult<string> Authorize([FromBody] UserLoginReq loginReq)
        {
            if (loginReq.Username == null && loginReq.Email == null)
            {
                return BadRequest(ResAnswers.BadRequest);
            }
            User? user = string.IsNullOrEmpty(loginReq.Email) ? context.Users.FirstOrDefault(x => x.Username.Equals(loginReq.Username!.ToLower())) : context.Users.FirstOrDefault(x => x.Email.Equals(loginReq.Email.ToLower()));
            if (user == null || !AuthTools.VerifyPassword(loginReq.Password, user.PasswordHash!, Convert.FromHexString(user.Salt!), _authData))
            {
                return BadRequest(ResAnswers.BadRequest);
            }
            return Ok(AuthTools.CreateToken(user, _authData));
        }
        /// <summary>
        /// See <see cref="IsEmailExist"/>
        /// </summary>
        /// <returns>200: Result of check: <c>true</c> - email exist, <c>false</c> - email doesn't exist.</returns>
        [HttpGet("CheckEmail")]
        public ActionResult<bool> CheckEmail(string email)
        {
            return Ok(IsEmailExist(email));
        }
        /// <summary>
        /// See <see cref="IsUsernameExist"/>
        /// </summary>
        /// <returns>200: Result of check: <c>true</c> - username exist, <c>false</c> - username doesn't exist.</returns>
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
                string currentUsername = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value ?? throw new NullReferenceException(ResAnswers.WrongJWT);
                user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUsername)) ?? throw new NullReferenceException(ResAnswers.UserNorFound);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(user);
        }
        /// <summary>
        /// Registrate new user at database, using request: <seealso cref="UserCreationReq"/>.
        /// </summary>
        /// <param name="userCreation">Data for new user creation.</param>
        /// <returns>200: String represent JWT Token.</returns>
        [HttpPost("Registrate")]
        public async Task<ActionResult<string>> Registrate([FromBody] UserCreationReq userCreation)
        {
            ValidationContext valContext = new(userCreation);
            List<ValidationResult> valResults = [];
            if (!Validator.TryValidateObject(userCreation, valContext, valResults, true))
            {
                return BadRequest(valResults);
            }
            if (IsEmailExist(userCreation.Email) || IsUsernameExist(userCreation.Username))
            {
                return BadRequest(ResAnswers.EmailUsernameExist);
            }

            //check smtp email?

            byte[] passSalt = AuthTools.CreateSalt(_authData.SaltSize);
            User user = new()
            {
                Username = userCreation.Username.ToLower(),
                Email = userCreation.Email.ToLower(),
                RegistrationDate = DateTime.UtcNow,
                Salt = Convert.ToHexString(passSalt),
                PasswordHash = AuthTools.HashPasword(userCreation.Password, passSalt, _authData)
            };
            context.Add(user);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, user);
            return Ok(AuthTools.CreateToken(user, _authData));
        }
        /// <summary>
        /// Check is email exist in <b>User</b> table at current context.
        /// </summary>
        /// <returns>Result of check: <c>true</c> - email exist, <c>false</c> - email doesn't exist.</returns>
        private bool IsEmailExist(string email)
        {
            return context.Users.Any(u => u.Email.Equals(email.ToLower()));
        }
        /// <summary>
        /// Check is username exist in <b>User</b> table at current context.
        /// </summary>
        /// <returns>Result of check: <c>true</c> - username exist, <c>false</c> - username doesn't exist.</returns>
        private bool IsUsernameExist(string username)
        {
            return context.Users.Any(u => u.Username.Equals(username.ToLower()));
        }
    }
}
