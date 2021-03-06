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


open System.IO


open NUnit.Framework


open VengefulFi.Ipld
open VengefulFi.Ipld.Convert


open VengefulFi.Encryption
open VengefulFi.Encryption.Compare
open VengefulFi.Encryption.Convert
open VengefulFi.Encryption.CryptoEngines
open VengefulFi.Encryption.Block


open VengefulFi.Encryption.Tests.Utils


open VengefulFi.Encryption.Tests.TestMappings


// extract
let keys =
    List.map
        System.Convert.FromBase64String
        [ "DAE4Cc4GYhvuEBhA5Uh6Cg==" // aes128
          "651/DGi4LoG4pqbCn7GiGw==" // aes128
          "JOC0K+KGVYkrvCLQvRLnDQ==" // aes128
          "MX3wIWZWNhFMMhiJtW/Odg==" // aes128
          "Dg7xN5W5jYAoagswFJf9TjMgmaOOeWOn" // aes192
          "PScsNjSTttbRE5zZ+mzUSJgYBFKtcxEF" // aes192
          "7yZbBzxjyEKsdOJuVa6IDiiWpc2ABaZh" // aes192
          "EpJojsP84iXg3lCpZEzcZJRf1fHdQQFL" // aes192
          "vSVYuh2AjtrsFdB68Iw/S+BYmqOhxsBd/upZbpH79so="
          "09R7vNdmrRb+eA54DWmHewTUuk268aZn3lZAu6kgGd4="
          "a7Q2A3VPi0dN3Y2mpYBNqq8edZz5MIO1ddZirwU5MXA="
          "44sGSiAdLGSn4IUWrFNg/DaoJr428/bFAq9+k61Wn9Y=" ]
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
    let plainBlock = getPlainMock plainContentId
    let cryptBlock = getRawMock encryptedCid

    let decryptedBlock = decryptBlock AesCbc aesKey cryptBlock

    // deconstruct
    let (PlainContent (GenericContent plainBlockBare)) = plainBlock
    let (PlainContent (GenericContent decryptedBlockBare)) = decryptedBlock

    // compare
    Assert.AreEqual(
        (fromStream plainBlockBare),
        (fromStream decryptedBlockBare)
    )


[<Test>]
let EncryptBlockTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    let plainBlock = getPlainMock plainContentId
    let cryptBlock = getRawMock encryptedCid

    let encryptedBlock = encryptBlock AesCbc aesKey plainBlock

    // deconstruct
    let (EncryptedContent (GenericContent cryptBlockBare)) = cryptBlock
    let (EncryptedContent (GenericContent encryptedBlockBare)) = encryptedBlock

    // compare
    Assert.AreEqual(
        (fromStream cryptBlockBare),
        (fromStream encryptedBlockBare)
    )


[<Test>]
let EncryptAndDecryptBlockTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    //let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    let plainBlock = getPlainMock plainContentId
    //let cryptBlock = getRawMock encryptedCid

    let decryptedBlock =
        plainBlock
        |> encryptBlock AesCbc aesKey
        |> decryptBlock AesCbc aesKey

    // deconstruct
    let (PlainContent (GenericContent plainBlockBare)) = plainBlock
    let (PlainContent (GenericContent decryptedBlockBare)) = decryptedBlock

    let plainBlockByte =
        plainBlockBare.Seek(0L, SeekOrigin.Begin)
        |> ignore

        fromStream plainBlockBare

    let decryptedBlockByte =
        plainBlockBare.Seek(0L, SeekOrigin.Begin)
        |> ignore

        fromStream decryptedBlockBare

    // compare
    Assert.AreEqual(plainBlockByte, decryptedBlockByte)


[<Test>]
let DecryptAndEncryptBlockTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    //let plainBlock = getPlainMock plainContentId
    let cryptBlock = getRawMock encryptedCid

    let encryptedBlock =
        cryptBlock
        |> decryptBlock AesCbc aesKey
        |> encryptBlock AesCbc aesKey

    // deconstruct
    let (EncryptedContent (GenericContent cryptBlockBare)) = cryptBlock
    let (EncryptedContent (GenericContent encryptedBlockBare)) = encryptedBlock

    let cryptBlockByte =
        cryptBlockBare.Seek(0L, SeekOrigin.Begin)
        |> ignore

        fromStream cryptBlockBare

    let encryptedBlockByte =
        cryptBlockBare.Seek(0L, SeekOrigin.Begin)
        |> ignore

        fromStream encryptedBlockBare

    // compare
    Assert.AreEqual(cryptBlockByte, encryptedBlockByte)


[<Test>]
let DecryptBlockFakeKeysTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    //let plainBlock = getPlainMock plainContentId
    let cryptBlock = getRawMock encryptedCid

    let subKey = Array.sub aesKey 0 (aesKey.Length - 12)
    let extKey = Array.zeroCreate<byte> (aesKey.Length + 12)

    Assert.Throws<IllegalKeySizeException> (fun () ->
        decryptBlock AesCbc subKey cryptBlock |> ignore)
    |> ignore

    Assert.Throws<IllegalKeySizeException> (fun () ->
        decryptBlock AesCbc extKey cryptBlock |> ignore)
    |> ignore


[<Test>]
let EncryptBlockFakeKeysTest
    ([<ValueSource(nameof (keys))>] aesKey: byte [])
    ([<ValueSource(nameof (cids))>] plainContentId: PlainContentId)
    : unit =

    // look up the corresponding encrypted cid
    //let encryptedCid = fetchCidMock testMappings AesCbc aesKey plainContentId

    // our starting points
    let plainBlock = getPlainMock plainContentId
    //let cryptBlock = getRawMock encryptedCid

    let subKey = Array.sub aesKey 0 (aesKey.Length - 12)
    let extKey = Array.zeroCreate<byte> (aesKey.Length + 12)

    Assert.Throws<IllegalKeySizeException> (fun () ->
        encryptBlock AesCbc subKey plainBlock |> ignore)
    |> ignore

    Assert.Throws<IllegalKeySizeException> (fun () ->
        encryptBlock AesCbc extKey plainBlock |> ignore)
    |> ignore
