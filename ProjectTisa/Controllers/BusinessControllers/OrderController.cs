using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPop.Controllers;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Models;

namespace ProjectTisa.Controllers.BusinessControllers
{
    /// <summary>
    /// CRU controller for <see cref="Order"/> model. <b>Required <see cref="AuthorizeAttribute"/> role</b> <c>Admin</c>/<c>Manager</c> or specified <c><see cref="User"/></c> claim on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<IEnumerable<Order>>> Get([FromQuery] PaginationRequest request)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            return Ok(request.ApplyRequest(await context.Orders.OrderBy(on => on.Id).ToListAsync()));
        }
        [HttpGet("{id}")]
        [Authorize]
        //add claim
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order? item = await context.Orders.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] OrderCreationReq request)
        {
            User? user = await context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            List<Product> products = await context.Products.Where(prd => request.ProductIds.Contains(prd.Id)).ToListAsync();
            Order order = new(request, new(User.Identity!.Name!), user, []);//test variant, need to be changed
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, order);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{order.Id}", ResAnswers.Created);
        }
        [HttpPut("{id}")]
        [Authorize]
        //add claim
        public async Task<ActionResult> Update(int id, [FromBody] OrderCreationReq request)
        {
            User? user = await context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            Order? toEdit = await context.Orders.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            List<Product> products = await context.Products.Where(prd => request.ProductIds.Contains(prd.Id)).ToListAsync();
            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Order fromOrder = new(request, toEdit.EditInfo, user, [], toEdit.Id);//test variant, need to be changed
            context.Entry(toEdit).CurrentValues.SetValues(fromOrder);
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
        private bool IsTableEmpty()
        {
            return context.Orders == null || !context.Orders.Any();
        }
    }
}
