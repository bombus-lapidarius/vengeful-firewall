using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class DhtController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Findpeer()
        {
            // this is just for demonstration purposes
            return new string[] { "dht", "Findpeer" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Findprovs()
        {
            // this is just for demonstration purposes
            return new string[] { "dht", "Findprovs" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Get()
        {
            // this is just for demonstration purposes
            return new string[] { "dht", "Get" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Provide()
        {
            // this is just for demonstration purposes
            return new string[] { "dht", "Provide" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Put()
        {
            // this is just for demonstration purposes
            return new string[] { "dht", "Put" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Query()
        {
            // this is just for demonstration purposes
            return new string[] { "dht", "Query" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
