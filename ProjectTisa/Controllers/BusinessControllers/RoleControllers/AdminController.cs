using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTisa.Controllers.BusinessControllers.RoleControllers
{
    /// <summary>
    /// Controller with methods for Admin role. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>admin</c>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "admin")]
    public class AdminController() : ControllerBase
    {

    }
}
