using Microsoft.EntityFrameworkCore;
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
        /// <returns><see cref="User"/> from current context.</returns>
        /// <exception cref="ControllerException">If JWT is wrong or user not found.</exception>
        public static async Task<User> GetUserFromContext(HttpContext httpContext, MainDbContext dbContext)
        {
            string currentUsername = httpContext.User.Identity!.Name ?? throw new ControllerException(ResAnswers.WrongJWT);
            User user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username.Equals(currentUsername)) ?? throw new ControllerException(ResAnswers.UserNorFound);
            user.LastSeen = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            return user;
        }
    }
}
