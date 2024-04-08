using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Models;
using ProjectTisa.Models.Enums;
using ProjectTisa.Controllers.GeneralData.Responses;

namespace ProjectTisa.Controllers.BusinessControllers.CrudControllers
{
    /// <summary>
    /// CRU controller for <see cref="Order"/> model. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> or specified <c><see cref="User"/></c> claim on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(ILogger<OrderController> logger, MainDbContext context, IAuthorizationService _authorizationService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IEnumerable<Order>>> Get([FromQuery] PaginationRequest request, [FromQuery] OrderStatus? status = null) =>
            Ok(await request.ApplyRequest(context.Orders.OrderBy(on => on.Id).Where(x => status == null || x.Status == status)));

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order? item = await context.Orders.FindAsync(id);
            if (item == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            if (item.User.Username != User.Identity!.Name! && !(await _authorizationService.AuthorizeAsync(User, "manage")).Succeeded)
            {
                return Forbid();
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IdResponse>> Create([FromBody] OrderCreationReq request)
        {
            User? user = await context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            if (user.Username != User.Identity!.Name)
            {
                return Forbid();
            }

            if (request.ProductIdQuantities.Count == 0)
            {
                return BadRequest(new MessageResponse(ResAnswers.BadRequest));
            }

            List<Product> productsInOrder = context.Products.AsEnumerable().Where(p => request.ProductIdQuantities.Any(pq => pq.ProductId.Equals(p.Id))).ToList();
            List<ProductQuantity> productQuantities = [.. request.ProductIdQuantities.Aggregate(new List<ProductQuantity>(), (list, pq) =>
            {
                Product? product = productsInOrder.FirstOrDefault(p => p.Id == pq.ProductId);
                if (product != null)
                {
                    list.Add(new ProductQuantity() { Product = product, Quantity = pq.Quantity });
                }
                return list;
            })];
            Order order = new(request, new(User.Identity!.Name!), user, productQuantities);//test variant, need to be changed
            context.Add(order);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, order);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{order.Id}", new IdResponse(order.Id));
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Update(int id, [FromBody] OrderCreationReq request)
        {
            User? user = await context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            Order? toEdit = await context.Orders.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            if (request.ProductIdQuantities.Count == 0 || toEdit.User.Id != request.UserId)
            {
                return BadRequest(new MessageResponse(ResAnswers.BadRequest));
            }

            IEnumerable<Product> productsInOrder = context.Products.AsEnumerable().Where(p => request.ProductIdQuantities.Any(pq => pq.ProductId.Equals(p.Id)));
            List<ProductQuantity> productQuantities = [.. request.ProductIdQuantities.Aggregate(new List<ProductQuantity>(), (list, pq) =>
            {
                Product? product = productsInOrder.FirstOrDefault(p => p.Id == pq.ProductId);
                if (product != null)
                {
                    list.Add(new ProductQuantity() { Product = product, Quantity = pq.Quantity });
                }
                return list;
            })];
            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Order fromOrder = new(request, toEdit.EditInfo, user, productQuantities, toEdit.TotalPrice, toEdit.Id);
            toEdit.ProductQuantities = productQuantities;
            toEdit.User = user;
            context.Entry(toEdit).CurrentValues.SetValues(fromOrder);
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        [HttpPatch("CompleteOrder")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> CompleteOrder(int id)
        {
            Order? order = await context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            if (order.Status != OrderStatus.InProgress)
            {
                return BadRequest(new MessageResponse(ResAnswers.BadRequest));
            }

            order.Status = OrderStatus.Completed;
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        [HttpPatch("CancelOrder")]
        [Authorize]
        public async Task<ActionResult<MessageResponse>> CancelOrder(int id)
        {
            Order? order = await context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            if (order.Status != OrderStatus.InProgress)
            {
                return BadRequest(new MessageResponse(ResAnswers.BadRequest));
            }

            if (order.User.Username != User.Identity!.Name && !(await _authorizationService.AuthorizeAsync(User, "manage")).Succeeded)
            {
                return Forbid();
            }

            order.Status = OrderStatus.Cancelled;
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
    }
}
