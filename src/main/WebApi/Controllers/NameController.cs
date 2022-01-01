using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class NameController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Publish()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Publish" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Resolve()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Resolve" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("pubsub/cancel")]
        public ActionResult<IEnumerable<string>> PubsubCancel()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Pubsub", "Cancel" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("pubsub/state")]
        public ActionResult<IEnumerable<string>> PubsubState()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Pubsub", "State" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("pubsub/subs")]
        public ActionResult<IEnumerable<string>> PubsubSubs()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Pubsub", "Subs" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
