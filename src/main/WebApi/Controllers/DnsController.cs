using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/{action=index}")] // TODO: trailing slash?
    [ApiController]
    public class DnsController: ControllerBase
    {
        // TODO: do not allow direct calls to Index()

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "dns", "Index" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
