namespace Newsweek.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Web.Models.News;
    
    public class NewsController : Controller
    {
        private readonly IQueryDispatcher queryDispatcher;

        public NewsController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var newsQuery = new SelectEntitiesQuery<News, NewsViewModel>() { Predicate = x => x.Id == id };
            var news = await queryDispatcher.Dispatch<SelectEntitiesQuery<News, NewsViewModel>, IEnumerable<NewsViewModel>>(newsQuery);

            return View(news.FirstOrDefault());
        }
    }
}