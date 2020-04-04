namespace Newsweek.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MediatR;
    
    using Microsoft.AspNetCore.Mvc;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Web.Models.Sources;
    
    public class SourcesController : Controller
    {
        private readonly IMediator mediator;

        public SourcesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            var selectQuery = new GetEntitiesQuery<Source, SourceFullViewModel>();
            IEnumerable<SourceFullViewModel> sources = await mediator.Send(selectQuery);

            return View(sources);
        }
    }
}