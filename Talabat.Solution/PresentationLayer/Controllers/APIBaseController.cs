using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class APIBaseController : ControllerBase
    {
        protected string GetEmailForToken() => User.FindFirstValue(ClaimTypes.Email)!;
    }
}
