using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/{action=index}")]
    [ApiController]
    public class BootstrapController: ControllerBase
    {
        // TODO: do not allow direct calls to Index()

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Index" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Add()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> AddDefault() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Add", "Default" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> List()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "List" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RmAll() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "bootstrap", "Rm", "All" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
