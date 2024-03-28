using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTisa.Controllers.BusinessControllers.RoleControllers
{
    /// <summary>
    /// Controller with methods for Admin role. <b>Required <see cref="AuthorizeAttribute"/> role:</b> <c>Admin</c>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController() : ControllerBase
    {

    }
}
