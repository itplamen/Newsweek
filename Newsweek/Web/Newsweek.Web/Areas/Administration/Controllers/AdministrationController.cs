namespace Newsweek.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Administration")]
    //[Authorize(Roles = "Administrator")]
    public abstract class AdministrationController : Controller
    {
    }
}