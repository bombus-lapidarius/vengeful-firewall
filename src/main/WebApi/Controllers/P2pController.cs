using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class P2pController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Close()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Close" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Forward()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Forward" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Listen()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Listen" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Ls" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("stream/close")]
        public ActionResult<IEnumerable<string>> StreamClose()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Stream", "Close" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("stream/ls")]
        public ActionResult<IEnumerable<string>> StreamLs()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Stream", "Ls" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
