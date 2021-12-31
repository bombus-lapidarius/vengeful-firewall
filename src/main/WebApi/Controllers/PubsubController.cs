using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")]
    [ApiController]
    public class PubsubController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Peers()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Peers" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Pub()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Pub" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Sub()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Sub" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
