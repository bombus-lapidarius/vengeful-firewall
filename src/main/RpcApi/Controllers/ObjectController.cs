// class VengefulFi.RpcApi.ObjectController


/* #############################################################################


Dual-licensed under MIT and Apache-2.0, by way of the [Permissive License
Stack](https://protocol.ai/blog/announcing-the-permissive-license-stack/).

Apache-2.0: https://www.apache.org/licenses/license-2.0
MIT: https://www.opensource.org/licenses/mit


################################################################################


Copyright 2021 Tomislav Petricevic

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.


################################################################################


MIT License

Copyright (c) 2021 Tomislav Petricevic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


############################################################################# */


using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace VengefulFi.RpcApi
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
} // namespace VengefulFi.RpcApi