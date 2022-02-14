module VengefulFi.Ipld.Codec


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


open VengefulFi.Ipld.Block


// TODO: move codecs to their own modules and only reference them here


type PbLink =
    { Hash: HashName * uint32 * Digest
      Name: string // TODO: ensure utf-8
      TargetSize: uint64 }


type PbNode =
    { Links: (HashName * uint32 * Digest) list // TODO: raw hash only?
      Data: byte [] }


// TODO: move codecs to their own modules and only reference them here


// TODO: use interface here?
let private newDagNodeCbor hashName hashSize cidVersion a = a
let private newDagNodeJson hashName hashSize cidVersion b = b
let private newDagNodePb hashName hashSize cidVersion c = c


exception UnsupportedCodecException of CidVersion * Multicodec


// TODO: use abstract DagNode type and just encode / decode into / from CBOR, JSON and ProtoBuf
let newDagNode
    (hashName: HashName)
    (hashSize: uint32)
    (cidVersion: CidVersion)
    (encoding: Multicodec)
    (data: byte [])
    =

    // TODO: verify compatibility of cid versions, codecs and hashes
    // TODO: include the list of links
    match encoding with
    | Multicodec.DagCbor -> newDagNodeCbor hashName hashSize cidVersion data
    | Multicodec.DagJson -> newDagNodeJson hashName hashSize cidVersion data
    | Multicodec.DagPb -> newDagNodePb hashName hashSize cidVersion data
    | _ -> raise (UnsupportedCodecException(cidVersion, encoding))
