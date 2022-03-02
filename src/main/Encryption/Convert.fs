module VengefulFi.Encryption.Convert


(* #############################################################################


Dual-licensed under MIT and Apache-2.0, by way of the [Permissive License
Stack](https://protocol.ai/blog/announcing-the-permissive-license-stack/).

Apache-2.0: https://www.apache.org/licenses/license-2.0
MIT: https://www.opensource.org/licenses/mit


################################################################################


Copyright 2022 Tomislav Petricevic

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

Copyright (c) 2022 Tomislav Petricevic

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


############################################################################# *)


open VengefulFi.Ipld
open VengefulFi.Ipld.Convert


// encoding conversions (base64)

let plainCidfromBase64 s =
    fromBase64 s |> RawContentId |> PlainContentId

let plainCidtoBase64 i =
    let (PlainContentId (RawContentId r)) = i
    toBase64 r

let encryptedCidfromBase64 s =
    fromBase64 s |> RawContentId |> EncryptedContentId

let encryptedCidtoBase64 i =
    let (EncryptedContentId (RawContentId r)) = i
    toBase64 r


// encoding conversions (hexstr)

let plainCidfromHexStr s =
    fromHexStr s |> RawContentId |> PlainContentId

let plainCidtoHexStr i =
    let (PlainContentId (RawContentId r)) = i
    toHexStr r

let encryptedCidfromHexStr s =
    fromHexStr s |> RawContentId |> EncryptedContentId

let encryptedCidtoHexStr i =
    let (EncryptedContentId (RawContentId r)) = i
    toHexStr r
