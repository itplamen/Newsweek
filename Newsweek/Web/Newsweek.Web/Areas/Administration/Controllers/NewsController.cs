namespace Newsweek.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;
    
    using Microsoft.AspNetCore.Mvc;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Delete;
    using Newsweek.Handlers.Queries.News.Search;
    using Newsweek.Web.ViewModels.News;

    public class NewsController : AdministrationController
    {
        private readonly IMediator mediator;

        public NewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("Administration/News")]
        [HttpGet("Administration/News/page/{page:int:min(1)}")]
        public async Task<IActionResult> Index(int page)
        {
            byte[] bytes = BitConverter.GetBytes(page);
            HttpContext.Session.Set("page", bytes);

            var searchNewsQuery = new SearchNewsQuery()
            {
                Page = page,
                NewsPerPage = 50
            };

            SearchNewsResponse searchNews = await mediator.Send(searchNewsQuery);
            SearchResponseViewModel response = Mapper.Map<SearchResponseViewModel>(searchNews);

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteEntityCommand<News>(id));

            HttpContext.Session.TryGetValue("page", out byte[] bytes);
            int page = BitConverter.ToInt32(bytes);

            return RedirectToAction("Index", "News", new { page });
        }
    }
}