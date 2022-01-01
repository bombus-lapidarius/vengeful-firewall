using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class SwarmController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Addrs()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("addrs/listen")]
        public ActionResult<IEnumerable<string>> AddrsListen()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs", "Listen" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("addrs/local")]
        public ActionResult<IEnumerable<string>> AddrsLocal()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs", "Local" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Connect()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Connect" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Disconnect()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Disconnect" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Filters()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("filters/add")]
        public ActionResult<IEnumerable<string>> FiltersAdd()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("filters/rm")]
        public ActionResult<IEnumerable<string>> FiltersRm()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters", "Rm" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("peering/add")]
        public ActionResult<IEnumerable<string>> PeeringAdd()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("peering/ls")]
        public ActionResult<IEnumerable<string>> PeeringLs()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Ls" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("peering/rm")]
        public ActionResult<IEnumerable<string>> PeeringRm()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Rm" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Peers()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peers" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
