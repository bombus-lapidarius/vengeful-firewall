using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")]
    [ApiController]
    public class BitswapController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ledger()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Ledger" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Reprovide()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Reprovide" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Stat" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Wantlist()
        {
            // this is just for demonstration purposes
            return new string[] { "bitswap", "Wantlist" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
