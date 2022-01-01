using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class BitswapController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Ledger()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Ledger" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Reprovide()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Reprovide" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Stat" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Wantlist()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Wantlist" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
