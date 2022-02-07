module VengefulFi.Encryption.Tests.Block


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


open NUnit.Framework


open VengefulFi.Encryption.Types
open VengefulFi.Encryption.Block


open VengefulFi.Encryption.Tests.Utils


open VengefulFi.Encryption.Tests.TestMappings
open VengefulFi.Encryption.Tests.TestDataList


// extract
let keys = List.map (fun x -> x.AesKey) testMappings
// extract
let cids =
    List.map (fun x -> x.Mappings) testMappings
    // create one big list of cid mappings
    |> List.concat
    // remove potential duplicates in the resulting list
    |> List.map (fun x -> x.PlainCid)
    |> List.distinctBy plainCidtoBase64


[<SetUp>] // nothing here yet
let Setup () = ()


[<Test>]
let DecryptBlockTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    let plainBlock = getPlainMock testDataList plainContentId
    let cryptBlock = getRawMock testDataList encryptedCid

    // perform the actual decryption
    let decryptedBlock = decryptBlock AesCbc aesKey cryptBlock

    // deconstruct
    let (PlainContent (GenericContent plainBlockBare)) = plainBlock
    let (PlainContent (GenericContent decryptedBlockBare)) = decryptedBlock

    // compare
    Assert.AreEqual((fromStream plainBlockBare), (fromStream decryptedBlockBare))


[<Test>]
let EncryptBlockTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    let plainBlock = getPlainMock testDataList plainContentId
    let cryptBlock = getRawMock testDataList encryptedCid

    // perform the actual encryption
    let encryptedBlock = encryptBlock AesCbc aesKey plainBlock

    // deconstruct
    let (EncryptedContent (GenericContent cryptBlockBare)) = cryptBlock
    let (EncryptedContent (GenericContent encryptedBlockBare)) = encryptedBlock

    // compare
    Assert.AreEqual((fromStream cryptBlockBare), (fromStream encryptedBlockBare))
