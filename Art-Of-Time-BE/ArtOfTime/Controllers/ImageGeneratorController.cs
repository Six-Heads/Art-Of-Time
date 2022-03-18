using Microsoft.AspNetCore.Mvc;

namespace ArtOfTime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageGeneratorController : ControllerBase
    {
        public string Index()
        {
            return "testsetests";
        }
    }
}
