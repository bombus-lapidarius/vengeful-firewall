using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/{action=index}")]
    [ApiController]
    public class RefsController: ControllerBase
    {
        // TODO: do not allow direct calls to Index()

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "refs", "Index" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Local()
        {
            // this is just for demonstration purposes
            return new string[] { "refs", "Local" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
