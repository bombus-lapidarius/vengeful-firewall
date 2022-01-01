using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")]
    [ApiController]
    public class ObjectController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Data()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Data" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Diff()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Diff" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Get()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Get" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Links()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Links" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> New()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "New" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Put()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Put" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Stat" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        [ActionName("add-link")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchAddLink() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "AddLink" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        [ActionName("append-data")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchAppendData() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "AppendData" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        [ActionName("rm-link")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchRmLink() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "RmLink" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        [ActionName("set-data")] // hyphen in URL
        public ActionResult<IEnumerable<string>> PatchSetData() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "object", "Patch", "SetData" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
