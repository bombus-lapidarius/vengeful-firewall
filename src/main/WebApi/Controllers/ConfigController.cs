using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/{action=index}")]
    [ApiController]
    public class ConfigController: ControllerBase
    {
        // TODO: do not allow direct calls to Index()

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Index" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> ProfileApply() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Profile", "Apply" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Edit()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Edit" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Replace()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Replace" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Show()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Show" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
