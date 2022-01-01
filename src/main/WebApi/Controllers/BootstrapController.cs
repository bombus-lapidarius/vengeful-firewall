using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class BootstrapController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost] // the default action
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Index" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Add()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("add/default")]
        public ActionResult<IEnumerable<string>> AddDefault()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Add", "Default" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> List()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "List" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Rm" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("rm/all")]
        public ActionResult<IEnumerable<string>> RmAll()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Rm", "All" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
