namespace Newsweek.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    
    using MediatR;
    
    using Microsoft.AspNetCore.Mvc;
    
    using Newsweek.Handlers.Queries.Dashboard;
    using Newsweek.Web.ViewModels.Areas.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly IMediator mediator;

        public DashboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            DashboardResult dashboardResult = await mediator.Send(new DashboardQuery());
            DashboardViewModel viewModel = Mapper.Map<DashboardViewModel>(dashboardResult);

            return View(viewModel);
        }
    }
}