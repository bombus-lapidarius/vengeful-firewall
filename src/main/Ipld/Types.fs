namespace VengefulFi.Ipld


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


open System.IO


type Multicodec =
    | DagCbor = 0x71ul
    | DagJson = 0x0129ul
    | DagPb = 0x70ul

type CidVersion =
    | Cid0 = 0x00ul // TODO: this may be incorrect
    | Cid1 = 0x01ul


type HashName =
    | Sha224 = 0x1013ul
    | Sha256 = 0x12ul
    | Sha384 = 0x20ul
    | Sha512 = 0x13ul

type Digest = Digest of byte []

type Hash =
    { Name: HashName
      Size: uint32
      Digest: Digest }


exception CidVersionAndCodecMismatchException of CidVersion * Multicodec
exception UnsupportedHashAlgorithmException of HashName * uint32
exception HashSizeMismatchException of HashName * uint32


type RawContentId = RawContentId of byte []
type RawContent = Stream


// TODO: struct?
type GenericContentIdFuture =
    { CidVersion: CidVersion
      Multicodec: Multicodec
      Hash: Hash }

type GenericContentId = RawContentId
type GenericContent = GenericContent of RawContent
