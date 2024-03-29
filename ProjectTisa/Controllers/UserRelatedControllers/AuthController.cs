using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectPop.Controllers;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Requests.UserReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;

namespace ProjectTisa.Controllers.UserRelatedControllers
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
        /// <param name="user"><see cref="User"/> that exist in database.</param>
        /// <returns>200: String represent JWT Token.</returns>
        [HttpPost("Authorize")]
        public async Task<ActionResult<string>> Authorize([FromBody] UserLoginReq loginReq)
        {
            if (loginReq.Username == null && loginReq.Email == null)
            {
                return BadRequest(ResAnswers.BadRequest);
            }
            User? user = string.IsNullOrEmpty(loginReq.Email) ? await context.Users.FirstOrDefaultAsync(x => x.Username.Equals(loginReq.Username!.ToLower())) : await context.Users.FirstOrDefaultAsync(x => x.Email.Equals(loginReq.Email.ToLower()));//revisit this solution
            if (user == null || !AuthTools.VerifyPassword(loginReq.Password, user.PasswordHash!, user.Salt!, _authData))
            {
                return BadRequest(ResAnswers.BadRequest);
            }
            return Ok(AuthTools.CreateToken(user, _authData));
        }
        /// <summary>
        /// <see cref="IsEmailExist"/>.
        /// </summary>
        /// <returns>200: Result of check: <c>true</c> - email exist, <c>false</c> - email doesn't exist.</returns>
        [HttpGet("CheckIsEmailExist")]
        public async Task<ActionResult<bool>> CheckIsEmailExist(string email) => Ok(await IsEmailExist(email));
        /// <summary>
        /// <see cref="IsUsernameExist"/>.
        /// </summary>
        /// <returns>200: Result of check: <c>true</c> - username exist, <c>false</c> - username doesn't exist.</returns>
        [HttpGet("CheckIsUsernameExist")]
        public async Task<ActionResult<bool>> CheckIsUsernameExist(string username) => Ok(await IsUsernameExist(username));
        /// <summary>
        /// Registrate new user at database's <b>PendingRegistration</b>, using request: <seealso cref="UserInfoReq"/>. 
        /// <para>See <seealso cref="Verify"/>.</para>
        /// </summary>
        /// <param name="userCreation">Data for new <see cref="User"/> creation.</param>
        /// <returns>200: Pending registration id.</returns>
        [HttpPost("Registrate")]
        public async Task<ActionResult<int>> Registrate([FromBody] UserInfoReq userCreation)
        {
            if (await IsEmailExist(userCreation.Email) || await IsUsernameExist(userCreation.Username))
            {
                return BadRequest(ResAnswers.EmailUsernameExist);
            }

            string verificationCode = AuthTools.GenerateCode();
            string passSalt = AuthTools.CreateSalt(_authData.SaltSize);
            PendingRegistration pendingReg = new(userCreation, passSalt, AuthTools.HashPasword(userCreation.Password, passSalt, _authData), verificationCode);
            context.Add(pendingReg);
            await context.SaveChangesAsync();
            EmailSender.SendEmailCode(userCreation.Email, verificationCode);
            LogMessageCreator.CreatedMessage(logger, pendingReg);
            return Ok(pendingReg.Id);
        }
        /// <summary>
        /// Verify pending registration request by sending code. If code same in table - create new <see cref="User"/>.
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
        /// Check is email exist in <see cref="User"/> table at current context.
        /// </summary>
        /// <returns>Result of check: <c>true</c> - email exist, <c>false</c> - email doesn't exist.</returns>
        private async Task<bool> IsEmailExist(string email)
        {
            return await context.Users.AnyAsync(u => u.Email.Equals(email.ToLower()));
        }
        /// <summary>
        /// Check is username exist in <see cref="User"/> table at current context.
        /// </summary>
        /// <returns>Result of check: <c>true</c> - username exist, <c>false</c> - username doesn't exist.</returns>
        private async Task<bool> IsUsernameExist(string username)
        {
            return await context.Users.AnyAsync(u => u.Username.Equals(username.ToLower()));
        }
    }
}
