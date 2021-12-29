using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class TarController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Add()
        {
            // this is just for demonstration purposes
            return new string[] { "tar", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Cat()
        {
            // this is just for demonstration purposes
            return new string[] { "tar", "Cat" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
