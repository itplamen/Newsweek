namespace Newsweek.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    
    using MediatR;
    
    using Microsoft.AspNetCore.Mvc;
    
    using Newsweek.Handlers.Queries.Dashboard;
    using Newsweek.Web.Areas.Administration.Models.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly IMediator mediator;

        public DashboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DashboardResult dashboardResult = await mediator.Send(new DashboardQuery());
            DashboardViewModel viewModel = Mapper.Map<DashboardViewModel>(dashboardResult);

            return View(viewModel);
        }
    }
}