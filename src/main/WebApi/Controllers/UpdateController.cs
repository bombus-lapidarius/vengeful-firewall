using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class UpdateController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost] // the default action
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "update", "Index" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
