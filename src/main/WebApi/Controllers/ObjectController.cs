using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class ObjectController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Data()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Data" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Diff()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Diff" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Get()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Get" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Links()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Links" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> New()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "New" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Put()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Put" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Stat" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("patch/add-link")]
        [ActionName("add-link")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchAddLink()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "AddLink" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("patch/append-data")]
        [ActionName("append-data")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchAppendData()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "AppendData" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("patch/rm-link")]
        [ActionName("rm-link")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchRmLink()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "RmLink" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("patch/set-data")]
        [ActionName("set-data")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchSetData()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "SetData" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
