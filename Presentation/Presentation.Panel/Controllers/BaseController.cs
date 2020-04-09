using System.Web.Mvc;
using Presentation.Panel.FilterAttributes;
using Presentation.Panel.Helpers;

namespace Presentation.Panel.Controllers
{
    [ErrorHandlerFilter]
    [AuthenticationFilter]
    public class BaseController : Controller
    {
        protected virtual CustomPrincipal LogedInAdmin => HttpContext.User as CustomPrincipal;
    }
}