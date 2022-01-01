using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class CidController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Base32()
        {
            // this is just for demonstration purposes
            return new string[] { "cid", "Base32" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Bases()
        {
            // this is just for demonstration purposes
            return new string[] { "cid", "Bases" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Codecs()
        {
            // this is just for demonstration purposes
            return new string[] { "cid", "Codecs" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Format()
        {
            // this is just for demonstration purposes
            return new string[] { "cid", "Format" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Hashes()
        {
            // this is just for demonstration purposes
            return new string[] { "cid", "Hashes" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
