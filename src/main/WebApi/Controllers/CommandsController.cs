using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class CommandsController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost] // the default action
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "commands", "Index" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("completion/bash")]
        public ActionResult<IEnumerable<string>> CompletionBash()
        {
            // this is just for demonstration purposes
            return new string[] { "commands", "Completion", "Bash" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
