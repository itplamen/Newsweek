namespace Newsweek.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using MediatR;
    
    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Handlers.Queries.News.Top;
    using Newsweek.Web.ViewModels.Common;
    using Newsweek.Web.ViewModels.News;

    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<NewsViewModel> topNews = await mediator.Send(new TopNewsQuery<NewsViewModel>(3));

            return View(topNews);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}