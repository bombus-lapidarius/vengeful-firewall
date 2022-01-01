using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class VersionController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost] // the default action
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "version", "Index" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Deps()
        {
            // this is just for demonstration purposes
            return new string[] { "version", "Deps" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
