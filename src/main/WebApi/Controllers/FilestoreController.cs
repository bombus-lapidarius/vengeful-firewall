using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class FilestoreController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Dups()
        {
            // this is just for demonstration purposes
            return new string[] { "filestore", "Dups" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "filestore", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Verify()
        {
            // this is just for demonstration purposes
            return new string[] { "filestore", "Verify" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
