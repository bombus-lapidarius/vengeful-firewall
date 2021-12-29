using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class StatsController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Bitswap()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Bitswap" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Bw()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Bw" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Dht()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Dht" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Provide()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Provide" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Repo()
        {
            // this is just for demonstration purposes
            return new string[] { "stats", "Repo" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
