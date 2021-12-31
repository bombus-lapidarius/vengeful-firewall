using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")]
    [ApiController]
    public class LogController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Level()
        {
            // this is just for demonstration purposes
            return new string[] { "log", "Level" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "log", "Ls" }; // TODO: async?
        }

        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Tail()
        {
            // this is just for demonstration purposes
            return new string[] { "log", "Tail" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
