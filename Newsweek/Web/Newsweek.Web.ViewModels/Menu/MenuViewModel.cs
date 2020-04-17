namespace Newsweek.Web.ViewModels.Menu
{
    using System.Collections.Generic;

    public class MenuViewModel
    {
        public string Name { get; set; }

        public IEnumerable<SubmenuViewModel> Submenu { get; set; }
    }
}