using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class P2pController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Close()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Close" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Forward()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Forward" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Listen()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Listen" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> StreamClose() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Stream", "Close" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> StreamLs() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "p2p", "Stream", "Ls" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
