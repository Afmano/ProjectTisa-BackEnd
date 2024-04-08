using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Controllers.GeneralData.Responses;

namespace ProjectTisa.Controllers.BusinessControllers.CrudControllers
{
    /// <summary>
    /// CRUD controller for <see cref="Discount"/> model. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController(ILogger<DiscountController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discount>>> Get([FromQuery] PaginationRequest request) =>
            Ok(await request.ApplyRequest(context.Discounts.OrderBy(on => on.Id)));
        [HttpGet("{id}")]
        public async Task<ActionResult<Discount>> Get(int id)
        {
            Discount? item = await context.Discounts.FindAsync(id);
            if (item == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IdResponse>> Create([FromBody] DiscountCreationReq request)
        {
            List<Product> products = await context.Products.Where(prd => request.ProductIds.Contains(prd.Id)).ToListAsync();
            Discount discount = new(request, new(User.Identity!.Name!), products);
            context.Add(discount);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, discount);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{discount.Id}", new IdResponse(discount.Id));
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Delete(int id)
        {
            Discount? item = await context.Discounts.FindAsync(id);
            if (item == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            context.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Update(int id, [FromBody] DiscountCreationReq request)
        {
            Discount? toEdit = await context.Discounts.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));

            }

            List<Product> products = await context.Products.Where(prd => request.ProductIds.Contains(prd.Id)).ToListAsync();
            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Discount fromDiscount = new(request, toEdit.EditInfo, products, toEdit.Id);
            toEdit.Products = products;
            context.Entry(toEdit).CurrentValues.SetValues(fromDiscount);
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
    }
}
