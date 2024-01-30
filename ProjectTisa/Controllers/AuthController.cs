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
        /// <summary>
        /// Get user's info from JWT token in Authorization header.
        /// </summary>
        /// <returns>200: JSON of user.</returns>
        [Authorize]
        [HttpGet("GetUser")]
        public ActionResult<string> GetUser()
        {
            User? user;
            try
            {
                string currentUsername = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value ?? throw new NullReferenceException(ResAnswers.WrongJWT);
                user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUsername)) ?? throw new NullReferenceException(ResAnswers.UserNorFound);
                user.LastSeen = DateTime.UtcNow;
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(user);
        }
        /// <summary>
        /// Registrate new user at database's <b>PendingRegistration</b>, using request: <seealso cref="UserCreationReq"/>. 
        /// <para>See <seealso cref="Verify"/>.</para>
        /// </summary>
        /// <param name="userCreation">Data for new user creation.</param>
        /// <returns>200: Pending registration id.</returns>
        [HttpPost("Registrate")]
        public async Task<ActionResult> Registrate([FromBody] UserCreationReq userCreation)
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

            string verificationCode = AuthTools.GenerateCode();
            byte[] passSalt = AuthTools.CreateSalt(_authData.SaltSize);
            PendingRegistration pendingReg = new()
            {
                Username = userCreation.Username.ToLower(),
                Email = userCreation.Email.ToLower(),
                ExpireDate = DateTime.UtcNow.AddHours(2),
                Salt = Convert.ToHexString(passSalt),
                PasswordHash = AuthTools.HashPasword(userCreation.Password, passSalt, _authData),
                VerificationCode = verificationCode
            };
            context.Add(pendingReg);
            await context.SaveChangesAsync();
            EmailSender.SendEmailCode(userCreation.Email, verificationCode);
            //LogMessageCreator.CreatedMessage(logger, pendingReg);
            return Ok(pendingReg.Id);
        }
        /// <summary>
        /// Verify pending registration request by sending code. If code same in table - create new user.
        /// </summary>
        /// <param name="pendingRegId">Id of pending registration from <see cref="Registrate"/>.</param>
        /// <param name="code">Code sended to registration email address.</param>
        /// <returns>200: String represent JWT Token.</returns>
        [HttpPost("Verify")]
        public async Task<ActionResult<string>> Verify(int pendingRegId, string code)
        {
            PendingRegistration request = context.PendingRegistrations.First(r => r.Id == pendingRegId);
            if (request == null || request.ExpireDate < DateTime.UtcNow || request.VerificationCode != code)
            {
                return BadRequest(ResAnswers.BadRequest);
            }
            User user = new(request);
            context.Add(user);
            context.PendingRegistrations.Remove(request);
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
