using Microsoft.AspNetCore.Mvc;

namespace Store.API.Controllers
{
    [Route("/api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}