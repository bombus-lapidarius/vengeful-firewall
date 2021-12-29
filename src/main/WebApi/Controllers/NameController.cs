using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class NameController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Publish()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Publish" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Resolve()
        {
            // this is just for demonstration purposes
            return new string[] { "name", "Resolve" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> PubSubCancel() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "name", "PubSub", "Cancel" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> PubSubState() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "name", "PubSub", "State" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> PubSubSubs() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "name", "PubSub", "Subs" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
