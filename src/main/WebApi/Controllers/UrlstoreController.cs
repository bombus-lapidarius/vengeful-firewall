using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class UrlstoreController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Add()
        {
            // this is just for demonstration purposes
            return new string[] { "urlstore", "Add" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
