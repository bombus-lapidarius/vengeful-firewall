// class VengefulFi.WebApi.SwarmController


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
    public class SwarmController: ControllerBase
    {
        // use POST, as these are all remote procedure calls

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Addrs()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("addrs/listen")]
        public ActionResult<IEnumerable<string>> AddrsListen()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs", "Listen" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("addrs/local")]
        public ActionResult<IEnumerable<string>> AddrsLocal()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Addrs", "Local" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Connect()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Connect" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Disconnect()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Disconnect" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Filters()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("filters/add")]
        public ActionResult<IEnumerable<string>> FiltersAdd()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("filters/rm")]
        public ActionResult<IEnumerable<string>> FiltersRm()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Filters", "Rm" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("peering/add")]
        public ActionResult<IEnumerable<string>> PeeringAdd()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Add" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("peering/ls")]
        public ActionResult<IEnumerable<string>> PeeringLs()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Ls" }; // TODO: async?
        }

        // this endpoint is nested, specify its url explicitly
        [HttpPost("peering/rm")]
        public ActionResult<IEnumerable<string>> PeeringRm()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peering", "Rm" }; // TODO: async?
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<string>> Peers()
        {
            // this is just for demonstration purposes
            return new string[] { "swarm", "Peers" }; // TODO: async?
        }
    }
} // namespace VengefulFi.WebApi
