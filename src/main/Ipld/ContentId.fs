module VengefulFi.Ipld.ContentId


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


open System.Security.Cryptography


let verifyHashSize (name: HashName) (size: uint32) =
    match (name, size) with // list all valid combinations
    | (HashName.Sha224, 224u) -> ()
    | (HashName.Sha256, 256u) -> ()
    | (HashName.Sha384, 384u) -> ()
    | (HashName.Sha512, 512u) -> ()
    // this should be tried last
    | _ -> raise (HashSizeMismatchException(name, size))


let verifyCidVersionAndCodec (version: CidVersion) (codec: Multicodec) =
    match (version, codec) with // list all valid combinations
    | (CidVersion.Cid0, Multicodec.DagProtoBuf) -> ()
    | (CidVersion.Cid1, Multicodec.DagCbor) -> ()
    | (CidVersion.Cid1, Multicodec.DagJson) -> ()
    | (CidVersion.Cid1, Multicodec.DagProtoBuf) -> ()
    // this should be tried last
    | _ -> raise (CidVersionAndCodecMismatchException(version, codec))


let private hashData
    (hashName: HashName)
    (hashSize: uint32)
    (data: RawContent)
    : Digest =

    verifyHashSize hashName hashSize

    // select the correct algorithm to hash the data
    match hashName with
    | HashName.Sha256 ->
        use hasher = SHA256.Create()
        hasher.ComputeHash(data) |> Digest // TODO: reset cursor position?
    | HashName.Sha384 ->
        use hasher = SHA384.Create()
        hasher.ComputeHash(data) |> Digest // TODO: reset cursor position?
    | HashName.Sha512 ->
        use hasher = SHA512.Create()
        hasher.ComputeHash(data) |> Digest // TODO: reset cursor position?
    // this should be tried last
    | _ -> raise (UnsupportedHashAlgorithmException(hashName, hashSize))


let calculateCid
    (hashName: HashName)
    (hashSize: uint32)
    (cidVersion: CidVersion)
    (multicodec: Multicodec)
    (data: RawContent)
    =

    // TODO: verify compatibility of cid versions, codecs and hashes
    let digest = hashData hashName hashSize data

    { CidVersion = cidVersion
      Multicodec = multicodec
      Hash =
        { Name = hashName
          Size = hashSize
          Digest = digest } }
