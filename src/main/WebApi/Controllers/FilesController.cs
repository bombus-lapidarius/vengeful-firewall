using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]")] // common for all actions below
    [ApiController]
    public class FilesController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Chcid()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Chcid" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Cp()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Cp" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Flush()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Flush" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Ls" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Mkdir()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Mkdir" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Mv()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Mv" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Read()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Read" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Rm()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Rm" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Stat()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Stat" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Write()
        {
            // this is just for demonstration purposes
            return new string[] { "files", "Write" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
