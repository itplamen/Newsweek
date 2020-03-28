namespace Newsweek.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;
    
    using MediatR;
    
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Comments;
    using Newsweek.Handlers.Commands.Common.Create;
    using Newsweek.Handlers.Commands.Common.Delete;
    using Newsweek.Handlers.Commands.Common.Update;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Web.Attributes;
    using Newsweek.Web.Models.Comments;

    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IMediator mediator;

        public CommentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [AjaxOnly]
        [AllowAnonymous]
        public async Task<IActionResult> Get(GetCommentsListViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentsQuery = new SelectEntitiesQuery<Comment, GetCommentViewModel>() 
            {
                Take = 3,
                Predicate = x => x.Id > request.Id && x.NewsId == request.NewsId 
            };
            IEnumerable<GetCommentViewModel> response = await mediator.Send(commentsQuery);

            return PartialView("_CommentsListPartial", response);
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<IActionResult> Post(CreateCommentViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            CreateCommentCommand command = Mapper.Map<CreateCommentCommand>(request);
            command.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<Comment> comments = await mediator.Send(new CreateEntitiesCommand<Comment>(Enumerable.Repeat(command, 1)));
            GetCommentViewModel response = Mapper.Map<GetCommentViewModel>(comments.First());
            response.Username = User.Identity.Name;

            return PartialView("_CommentPartial", response);
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<IActionResult> Update(UpdateCommentViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isUpdated = await mediator.Send(new UpdateEntityCommand<Comment, UpdateCommentViewModel>(request));

            return Ok(isUpdated);
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid comment Id!");
            }

            bool isDeleted = await mediator.Send(new DeleteEntityCommand<Comment>(id));

            return Ok(isDeleted);
        }
    }
}