using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class FilesController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Chcid()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Chcid" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Cp()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Cp" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Flush()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Flush" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Mkdir()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Mkdir" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Mv()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Mv" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Read()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Read" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Rm" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Stat" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Write()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Write" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
