using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class DagController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Export()
        {
            // this is just for demonstration purposes
            return new string[] { "dag", "Export" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Get()
        {
            // this is just for demonstration purposes
            return new string[] { "dag", "Get" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Import()
        {
            // this is just for demonstration purposes
            return new string[] { "dag", "Import" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Put()
        {
            // this is just for demonstration purposes
            return new string[] { "dag", "Put" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Resolve()
        {
            // this is just for demonstration purposes
            return new string[] { "dag", "Resolve" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "dag", "Stat" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
