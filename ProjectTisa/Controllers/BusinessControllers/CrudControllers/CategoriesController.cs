using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;

namespace ProjectTisa.Controllers.BusinessControllers.CrudControllers
{
    /// <summary>
    /// CRUD controller for <see cref="Category"/> model. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ILogger<CategoriesController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromQuery] PaginationRequest request, bool haveParent = true) =>
            Ok(await request.ApplyRequestAsync(context.Categories.OrderBy(on => on.Id).Where(x => x.ParentCategory == null || haveParent)));
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            Category? item = await context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IdResponse>> Create([FromBody] CategoryCreationReq request)
        {
            Category? parentCategory = await context.Categories.FindAsync(request.ParentCategoryId);
            Category category = new(request, new(User.Identity!.Name!), parentCategory);
            context.Add(category);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, category);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{category.Id}", new IdResponse(category.Id));
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Delete(int id)
        {
            Category? item = await context.Categories.FindAsync(id);
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
        public async Task<ActionResult<MessageResponse>> Update(int id, [FromBody] CategoryCreationReq request)
        {
            Category? toEdit = await context.Categories.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            Category? parentCategory = await context.Categories.FindAsync(request.ParentCategoryId);
            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Category fromCategory = new(request, toEdit.EditInfo, parentCategory, toEdit.Id);
            toEdit.ParentCategory = parentCategory;
            context.Entry(toEdit).CurrentValues.SetValues(fromCategory);
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
    }
}
