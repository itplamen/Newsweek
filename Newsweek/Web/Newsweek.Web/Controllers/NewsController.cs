namespace Newsweek.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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

        [HttpGet]
        public async Task<IActionResult> Search(SearchRequestViewModel request)
        {
            SearchResponseViewModel response = new SearchResponseViewModel();
            Expression<Func<News, bool>> expression = null;

            if (!string.IsNullOrEmpty(request.Category))
            {
                response.Search = request.Category;
                expression = x => x.Subcategory.Category.Name == request.Category;
            }

            if (!string.IsNullOrEmpty(request.Subcategory))
            {
                response.Search = request.Subcategory;
                expression = x => x.Subcategory.Name == request.Subcategory;
            }

            var newsQuery = new SelectEntitiesQuery<News, NewsViewModel>() { Predicate = expression };
            response.News = await queryDispatcher.Dispatch<SelectEntitiesQuery<News, NewsViewModel>, IEnumerable<NewsViewModel>>(newsQuery);

            return View(response);
        }
    }
}