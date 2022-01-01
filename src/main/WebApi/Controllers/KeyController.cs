using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class KeyController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Export()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "Export" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Gen()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "Gen" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Import()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "Import" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> List()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "List" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Rename()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "Rename" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "Rm" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Rotate()
        {
            // this is just for demonstration purposes
            return new string[] { "key", "Rotate" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
