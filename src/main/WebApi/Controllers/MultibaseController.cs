using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class MultibaseController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Decode()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "Decode" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Encode()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "Encode" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> List()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "List" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Transcode()
        {
            // this is just for demonstration purposes
            return new string[] { "multibase", "Transcode" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
