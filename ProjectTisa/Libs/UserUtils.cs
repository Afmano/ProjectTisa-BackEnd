using ProjectTisa.Controllers.GeneralData.Exceptions;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Models;

namespace ProjectTisa.Libs
{
    public class UserUtils
    {

        /// <summary>
        /// Reading JWT token from Authorization header return <see cref="User"/> from database.
        /// </summary>
        /// <param name="httpContext">Current HttpContext from controller.</param>
        /// <param name="dbContext">Current database context from application.</param>
        /// <returns><see cref="User"/> class.</returns>
        /// <exception cref="ControllerException">If JWT is wrong or user not found.</exception>
        public static User GetUserFromContext(HttpContext httpContext, MainDbContext dbContext)
        {
            User user;
            string currentUsername = httpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value ?? throw new ControllerException(ResAnswers.WrongJWT);
            user = dbContext.Users.FirstOrDefault(x => x.Username.Equals(currentUsername)) ?? throw new ControllerException(ResAnswers.UserNorFound);
            user.LastSeen = DateTime.UtcNow;
            return user;
        }
    }
}
