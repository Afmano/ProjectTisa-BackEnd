using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Controllers.GeneralData.Responses;

namespace ProjectTisa.Controllers.BusinessControllers.CrudControllers
{
    /// <summary>
    /// CRUD controller for <see cref="Product"/> model. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ILogger<ProductController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] PaginationRequest request, bool onlyActive = true) =>
            Ok(await request.ApplyRequest(context.Products.OrderBy(on => on.Id).Where(x => x.IsAvailable || !onlyActive)));
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            Product? item = await context.Products.FindAsync(id);
            if (item == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            return Ok(item);
        }
        [HttpGet("GetAllByCategory")]
        public async Task<ActionResult<List<Product>>> GetAllByCategory([FromQuery] int? categoryId = null, [FromQuery] string? categoryName = null) =>
            Ok(await Task.Run(() => context.Products.Where(product => product.Category.Id == categoryId || product.Category.Name == categoryName).ToList()));
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IdResponse>> Create([FromBody] ProductCreationReq request)
        {
            Discount? discount = await context.Discounts.FindAsync(request.DiscountId);
            Category? category = await context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            Product product = new(request, new(User.Identity!.Name!), category, discount);
            context.Add(product);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, product);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{product.Id}", new IdResponse(product.Id));
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Delete(int id)
        {
            Product? item = await context.Products.FindAsync(id);
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
        public async Task<ActionResult<MessageResponse>> Update(int id, [FromBody] ProductCreationReq request)
        {
            Product? toEdit = await context.Products.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            Discount? discount = await context.Discounts.FindAsync(request.DiscountId);
            Category? category = await context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Product fromProduct = new(request, toEdit.EditInfo, category, discount, toEdit.Id);
            toEdit.Discount = discount;
            toEdit.Category = category;
            context.Entry(toEdit).CurrentValues.SetValues(fromProduct);
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
    }
}
