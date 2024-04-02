using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;

namespace ProjectTisa.Controllers.BusinessControllers.CrudControllers
{
    /// <summary>
    /// CRUD controller for <see cref="Category"/> model. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromQuery] PaginationRequest request, bool haveParent = true) =>
            Ok(await request.ApplyRequest(context.Categories.OrderBy(on => on.Id).Where(x => x.ParentCategory == null || haveParent)));
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            Category? item = await context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Create([FromBody] CategoryCreationReq request)
        {
            Category? parentCategory = await context.Categories.FindAsync(request.ParentCategoryId);
            Category category = new(request, new(User.Identity!.Name!), parentCategory);
            context.Add(category);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, category);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{category.Id}", ResAnswers.Created);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            Category? item = await context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            context.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);

            return Ok(ResAnswers.Success);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Update(int id, [FromBody] CategoryCreationReq request)
        {
            Category? toEdit = await context.Categories.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            Category? parentCategory = await context.Categories.FindAsync(request.ParentCategoryId);
            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Category fromCategory = new(request, toEdit.EditInfo, parentCategory, toEdit.Id);
            toEdit.ParentCategory = parentCategory;
            context.Entry(toEdit).CurrentValues.SetValues(fromCategory);
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
    }
}
