using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ForcefulFi.WebApi
{
    [Route("api/v0/[controller]/[action]")] // TODO: trailing slash?
    [ApiController]
    public class FileController: ControllerBase
    {
        [HttpPost] // use POST, as this is a remote procedure call
        public ActionResult<IEnumerable<string>> Ls()
        {
            // this is just for demonstration purposes
            return new string[] { "file", "Ls" }; // TODO: async?
        }
    }
} // namespace ForcefulFi.WebApi
