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
    /// <summary>
    /// CRUD controller for <see cref="Product"/> model. <b>Required <see cref="AuthorizeAttribute"/> role:</b> <c>Admin</c> or <c>Manager</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] PaginationRequest request, bool onlyActive)//add
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            return Ok(request.ApplyRequest(await context.Products.OrderBy(on => on.Id).ToListAsync()).Where(x => x.IsAvailable || !onlyActive));
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
        [HttpGet("GetAllByCategory")]
        public async Task<ActionResult<List<Product>>> GetAllByCategory(int? categoryid, string? categoryName) =>
            Ok(await Task.Run(() => context.Products.Where(product => product.Category.Id == categoryid || product.Category.Name == categoryName).ToList()));
        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<string>> Create([FromBody] ProductCreationReq request)
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
        public async Task<ActionResult<string>> Delete(int id)
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
        public async Task<ActionResult<string>> Update(int id, [FromBody] ProductCreationReq request)
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
            Product fromProduct = new(request, toEdit.EditInfo, category, discount, toEdit.Id);
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
