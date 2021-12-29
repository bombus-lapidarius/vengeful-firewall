using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class RepoController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Fsck()
        {
            // this is just for demonstration purposes
            return new string[] { "repo", "Fsck" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Gc()
        {
            // this is just for demonstration purposes
            return new string[] { "repo", "Gc" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "repo", "Stat" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Verify()
        {
            // this is just for demonstration purposes
            return new string[] { "repo", "Verify" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Version()
        {
            // this is just for demonstration purposes
            return new string[] { "repo", "Version" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
