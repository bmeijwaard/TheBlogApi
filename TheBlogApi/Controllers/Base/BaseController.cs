using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheBlogApi.Controllers.Base
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class BaseController : Controller
    {
    }
}
