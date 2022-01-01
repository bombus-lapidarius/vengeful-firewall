using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class DiagController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Cmds()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Cmds" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("cmds/clear")]
        public ActionResult<IEnumerable<string>> CmdsClear()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Cmds", "Clear" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("cmds/set-time")]
        [ActionName("set-time")] // hyphen in URL
        public ActionResult<IEnumerable<string>> CmdsSetTime()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Cmds", "SetTime" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Profile()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Profile" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Sys()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Sys" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
