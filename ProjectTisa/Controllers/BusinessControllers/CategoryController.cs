using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPop.Controllers;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;

namespace ProjectTisa.Controllers.BusinessControllers
{
    /// <summary>
    /// CRUD controller for <see cref="Category"/> model. <b>Required <see cref="AuthorizeAttribute"/> role:</b> <c>Admin</c> or <c>Manager</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromQuery] PaginationRequest request, bool haveParent)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            return Ok(request.ApplyRequest(await context.Categories.OrderBy(on => on.Id).ToListAsync()).Where(x => x.ParentCategory == null || haveParent));
        }
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
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> Create([FromBody] CategoryCreationReq request)
        {
            Category? parentCategory = await context.Categories.FindAsync(request.ParentCategoryId);
            Category category = new(request, new(User.Identity!.Name!), parentCategory);
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, category);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{category.Id}", ResAnswers.Created);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            Category? item = await context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            context.Categories.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);

            return Ok(ResAnswers.Success);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] CategoryCreationReq request)
        {
            Category? toEdit = await context.Categories.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }
            
            Category? parentCategory = await context.Categories.FindAsync(request.ParentCategoryId);
            if (parentCategory == null)
            {
                return BadRequest(ResAnswers.NotFoundNullEntity);
            }

            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Category fromCategory = new(request, toEdit.EditInfo, parentCategory);
            context.Entry(toEdit).CurrentValues.SetValues(fromCategory);
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
        private bool IsTableEmpty()
        {
            return context.Categories == null || !context.Categories.Any();
        }
    }
}
