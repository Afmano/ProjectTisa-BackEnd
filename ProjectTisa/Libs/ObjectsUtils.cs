using ProjectTisa.Controllers.GeneralData.Exceptions;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Libs
{
    public class ObjectsUtils
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
            User user;
            string currentUsername = httpContext.User.Identity!.Name ?? throw new ControllerException(ResAnswers.WrongJWT);
            user = dbContext.Users.FirstOrDefault(x => x.Username.Equals(currentUsername)) ?? throw new ControllerException(ResAnswers.UserNorFound);
            user.LastSeen = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            return user;
        }
        /// <summary>
        /// Validate DTO by validate attributes.
        /// </summary>
        /// <param name="item">Object to validate.</param>
        /// <returns>List of validation results.</returns>
        /// /// <exception cref="ArgumentNullException">If object is null.</exception>
        public static List<ValidationResult> Validate<T>(T item)
        {
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            ValidationContext valContext = new(item);
            List<ValidationResult> valResults = [];
            Validator.TryValidateObject(item, valContext, valResults, true);
            return valResults;
        }
    }
}
