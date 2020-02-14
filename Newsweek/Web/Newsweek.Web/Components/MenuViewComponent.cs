namespace Newsweek.Web.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Web.Models.Menu;
   
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly IQueryDispatcher queryDispatcher;

        public MenuViewComponent(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
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

            IEnumerable<MenuViewModel> menu = await queryDispatcher
                .Dispatch<SelectEntitiesQuery<Category, MenuViewModel>, IEnumerable<MenuViewModel>>(query);

            return View(menu);
        }
    }
}