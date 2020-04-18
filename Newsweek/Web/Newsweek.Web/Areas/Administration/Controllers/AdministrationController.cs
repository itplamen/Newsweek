namespace Newsweek.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Newsweek.Common.Roles;

    [Area("Administration")]
    [Authorize(Roles = ApplicationRoles.ADMINISTRATOR)]
    public abstract class AdministrationController : Controller
    {
    }
}