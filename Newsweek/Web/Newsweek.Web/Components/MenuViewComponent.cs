namespace Newsweek.Web.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Web.Models.Menu;
   
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public MenuViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new SelectEntitiesQuery<Category, MenuViewModel>()
            {
                Predicate = x => !x.IsDeleted,
                Selector = x => new MenuViewModel()
                {
                    Name = x.Name,
                    Submenu = x.Subcategories.Select(y => new SubmenuViewModel()
                    {
                        Name = y.Name
                    })
                    .OrderBy(x => x.Name)
                }
            };

            IEnumerable<MenuViewModel> menu = await mediator.Send(query);

            return View(menu);
        }
    }
}