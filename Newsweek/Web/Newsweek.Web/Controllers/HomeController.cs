namespace Newsweek.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Web.Models.Common;
    using Newsweek.Web.Models.News;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IQueryDispatcher queryDispatcher;

        public HomeController(ILogger<HomeController> logger, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<NewsViewModel> topNews = await queryDispatcher.Dispatch<IEnumerable<NewsViewModel>>();

            return View(topNews);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}