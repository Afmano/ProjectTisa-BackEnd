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

namespace ProjectTisa.Controllers.BusinessControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] PaginationRequest request)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            return Ok(request.ApplyRequest(await context.Products.OrderBy(on => on.Id).ToListAsync()));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            Product? item = await context.Products.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> Create([FromBody] ProductCreationReq request)
        {
            Discount? discount = await context.Discounts.FindAsync(request.DiscountId);
            Category? category = await context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest(ResAnswers.NotFoundNullEntity);
            }

            Product product = new(request, new(User.Identity!.Name!), category, discount);
            context.Products.Add(product);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, product);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{product.Id}", ResAnswers.Created);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            Product? item = await context.Products.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            context.Products.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);
            return Ok(ResAnswers.Success);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductCreationReq request)
        {
            Product? toEdit = await context.Products.FindAsync(id);
            if(toEdit == null) 
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            Discount? discount = await context.Discounts.FindAsync(request.DiscountId);
            Category? category = await context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest(ResAnswers.NotFoundNullEntity);
            }

            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Product fromProduct = new(request, toEdit.EditInfo, category, discount);
            context.Entry(toEdit).CurrentValues.SetValues(fromProduct);
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
        private bool IsTableEmpty()
        {
            return context.Products == null || !context.Products.Any();
        }
    }
}
