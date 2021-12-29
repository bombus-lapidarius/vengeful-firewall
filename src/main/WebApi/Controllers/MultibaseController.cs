using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class MultibaseController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Decode()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "Decode" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Encode()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "Encode" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> List()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "List" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Transcode()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "Transcode" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
