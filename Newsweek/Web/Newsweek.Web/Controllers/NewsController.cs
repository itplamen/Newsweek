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
            var newsQuery = new GetEntitiesQuery<News, NewsViewModel>() { Predicate = x => x.Id == id };
            IEnumerable<NewsViewModel> news = await mediator.Send(newsQuery);

            return View(news.FirstOrDefault());
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchRequestViewModel request)
        {
            SearchResponseViewModel response = new SearchResponseViewModel();
            Expression<Func<News, bool>> expression = null;

            if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                response.Search = request.Tag;
                expression = x => x.Tags.Any(y => y.Tag.Name == request.Tag.ToLower());
            }
            else
            {
                response.Search = $"{request.Category}";
                expression = x => x.Subcategory.Category.Name == request.Category;

                if (!string.IsNullOrWhiteSpace(request.Subcategory))
                {
                    response.Search += $"{response.Search}/{request.Subcategory}";
                    expression = x => x.Subcategory.Category.Name == request.Category && x.Subcategory.Name == request.Subcategory;
                }
            }

            var searchQeury = new GetEntitiesQuery<News, NewsViewModel>() 
            { 
                Take = request.Take, 
                Predicate = expression,
                OrderBy = x => x.OrderByDescending(y => y.Id)
            };

            response.News = await mediator.Send(searchQeury);

            return View(response);
        }
    }
}