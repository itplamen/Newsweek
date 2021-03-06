﻿namespace Newsweek.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.News.Search;
    using Newsweek.Web.ViewModels.News;

    public class NewsController : Controller
    {
        private readonly IMediator mediator;

        public NewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("News/{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            var newsQuery = new GetEntitiesQuery<News, NewsViewModel>() { Predicate = x => x.Id == id };
            IEnumerable<NewsViewModel> news = await mediator.Send(newsQuery);

            return View(news.FirstOrDefault());
        }

        [HttpGet("News/Search")]
        [HttpGet("News/{category}")]
        [HttpGet("News/{category}/{subcategory}")]
        [HttpGet("News/{category}/page/{page:int:min(1)}")]
        [HttpGet("News/{category}/{subcategory}/page/{page:int:min(1)}")]
        public async Task<IActionResult> Search(SearchRequestViewModel request)
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Tag = request.Tag,
                Category = request.Category,
                Subcategory = request.Subcategory,
                Page = request.Page,
                NewsPerPage = 9
            };

            SearchNewsResponse searchNews = await mediator.Send(searchNewsQuery);
            SearchResponseViewModel response = Mapper.Map<SearchResponseViewModel>(searchNews);

            return View(response);
        }
    }
}