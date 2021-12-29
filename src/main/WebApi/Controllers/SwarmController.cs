using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class SwarmController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Addrs()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> AddrsListen() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs", "Listen" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> AddrsLocal() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs", "Local" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Connect()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Connect" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Disconnect()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Disconnect" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Filters()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> FiltersAdd() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> FiltersRm() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> PeeringAdd() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> PeeringLs() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> PeeringRm() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Peers()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peers" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
