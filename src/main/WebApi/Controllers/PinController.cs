using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class PinController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Add()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Add" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Ls" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Rm" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("remote/add")]
        public ActionResult<IEnumerable<string>> RemoteAdd()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("remote/ls")]
        public ActionResult<IEnumerable<string>> RemoteLs()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Ls" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("remote/rm")]
        public ActionResult<IEnumerable<string>> RemoteRm()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Rm" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("remote/service/add")]
        public ActionResult<IEnumerable<string>> RemoteServiceAdd()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Service", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("remote/service/ls")]
        public ActionResult<IEnumerable<string>> RemoteServiceLs()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Service", "Ls" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("remote/service/rm")]
        public ActionResult<IEnumerable<string>> RemoteServiceRm()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Remote", "Service", "Rm" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Update()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Update" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Verify()
        {
            // this is just for demonstration purposes
            return new string[] { "pin", "Verify" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
