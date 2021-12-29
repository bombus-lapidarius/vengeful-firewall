using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class DiagController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Cmds()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Cmds" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> CmdsClear() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Cmds", "Clear" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> CmdsSetTime() // TODO: nested URL // TODO: hyphen in URL
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Cmds", "SetTime" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Profile()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Profile" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Sys()
        {
            // this is just for demonstration purposes
            return new string[] { "diag", "Sys" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
