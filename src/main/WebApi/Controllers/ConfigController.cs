using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class ConfigController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost] // the default action
        public ActionResult<IEnumerable<string>> Index()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Index" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("profile/apply")]
        public ActionResult<IEnumerable<string>> ProfileApply()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Profile", "Apply" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Edit()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Edit" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Replace()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Replace" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Show()
        {
            // this is just for demonstration purposes
            return new string[] { "config", "Show" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
