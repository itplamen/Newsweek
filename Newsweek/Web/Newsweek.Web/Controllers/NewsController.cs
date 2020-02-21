namespace Newsweek.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using MediatR;
    
    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Web.Models.News;
    
    public class NewsController : Controller
    {
        private readonly IMediator mediator;

        public NewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var newsQuery = new SelectEntitiesQuery<News, NewsViewModel>() { Predicate = x => x.Id == id };
            IEnumerable<NewsViewModel> news = await mediator.Send(newsQuery);

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

            response.News = await mediator.Send(new SelectEntitiesQuery<News, NewsViewModel>() { Predicate = expression });

            return View(response);
        }
    }
}