using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class PinController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Add()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RemoteAdd() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RemoteLs() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RemoteRm() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RemoteServiceAdd() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Service", "Add" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RemoteServiceLs() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Service", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> RemoteServiceRm() // TODO: nested URL
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Service", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Update()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Update" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Verify()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Verify" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
