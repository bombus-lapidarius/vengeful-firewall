using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class ConfigController: ControllerBase
    {
        // TODO: top level handler (api/v0/config)

        [HttpGet] // TODO: HTTP POST?
        public ActionResult<IEnumerable<string>> Apply() // TODO: complete path
        {
            // this is just for demonstration purposes
            return new string[] { "You", "put", "me", "here" }; // TODO: async?
        }

        [HttpGet] // TODO: HTTP POST?
        public ActionResult<IEnumerable<string>> Edit()
        {
            // this is just for demonstration purposes
            return new string[] { "I", "am", "a", "config", "file" }; // TODO: async?
        }

        [HttpGet] // TODO: HTTP POST?
        public ActionResult<IEnumerable<string>> Replace()
        {
            // this is just for demonstration purposes
            return new string[] { "You", "put", "me", "here" }; // TODO: async?
        }

        [HttpGet] // TODO: HTTP POST?
        public ActionResult<IEnumerable<string>> Show()
        {
            // this is just for demonstration purposes
            return new string[] { "I", "am", "a", "config", "file" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
