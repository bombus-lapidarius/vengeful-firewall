using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class StatsController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Bitswap()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Bitswap" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Bw()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Bw" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Dht()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Dht" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Provide()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Provide" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Repo()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Repo" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
