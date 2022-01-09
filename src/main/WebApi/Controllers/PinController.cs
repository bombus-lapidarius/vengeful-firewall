// class VengefulFi.WebApi.PinController


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

namespace VengefulFi.WebApi
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
} // namespace VengefulFi.WebApi
