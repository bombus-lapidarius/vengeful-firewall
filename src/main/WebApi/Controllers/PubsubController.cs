using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class PubsubController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Ls" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Peers()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Peers" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Pub()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Pub" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Sub()
        {
            // this is just for demonstration purposes
            return new string[] { "pubsub", "Sub" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
