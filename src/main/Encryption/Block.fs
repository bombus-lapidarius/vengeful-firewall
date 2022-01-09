module Encryption.Block


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


type PlainContent = Types.PlainContent
type EncryptedContent = Types.EncryptedContent


type PlainContentId = Types.PlainContentId
type EncryptedContentId = Types.EncryptedContentId


type Cipher = Types.Cipher


type CalcPlainCidType = PlainContent -> PlainContentId


// parameterise based on these function types to enable efficient unit testing
type FetchCidType = Cipher -> byte[] -> PlainContentId -> EncryptedContentId
type StoreCidType = Cipher -> byte[] -> PlainContentId -> EncryptedContentId -> unit


// parameterise based on these function types to enable efficient unit testing
type GetRawType = EncryptedContentId -> EncryptedContent
type PutRawType = EncryptedContent -> EncryptedContentId


let private createCryptoEngine (cipher: Cipher) (key: byte[]): Aes = // TODO: generalise
    match cipher with
    | Types.Cipher.AesCbc ->
        let aes = Aes.Create()
        // TODO: verify that the key size is valid for the chosen cipher
        aes.Key <- key
        // TODO: set block size?
        aes.IV <- key // TODO: essiv for IV generation
        aes.Mode <- CipherMode.CBC
        // TODO: set feedback size?
        // TODO: set padding?
        aes // cbc


let decryptBlock (cipher: Cipher) (key: byte[]) (input: EncryptedContent):
    PlainContent =

    use aes = createCryptoEngine cipher key // TODO: dangling ref?
    let cbcDecryptor = aes.CreateDecryptor()
    let (Types.EncryptedContent inputStream) = input
    let output = new CryptoStream(inputStream, cbcDecryptor, CryptoStreamMode.Read, false) // TODO: does this return a usable stream object?
    Types.PlainContent output // construct the tagged union result


let encryptBlock (cipher: Cipher) (key: byte[]) (input: PlainContent):
    EncryptedContent =

    use aes = createCryptoEngine cipher key // TODO: dangling ref?
    let cbcEncryptor = aes.CreateEncryptor()
    let (Types.PlainContent inputStream) = input
    let output = new CryptoStream(inputStream, cbcEncryptor, CryptoStreamMode.Read, false) // TODO: does this return a usable stream object?
    Types.EncryptedContent output // construct the tagged union result


let decryptGet (getRaw: GetRawType) (fetchCid: FetchCidType)
    (cache: Cache.PlainDataCache) (cipher: Cipher) (key: byte[])
    (plainCid: PlainContentId): PlainContent =

    match Cache.getFromCache cache plainCid with // PlainContent
    | None -> // we need to fetch the data from IPFS
        fetchCid cipher key plainCid // returns EncryptedContentId
        |> getRaw // returns EncryptedContent
        // TODO: hash here to verify that the encrypted content returned matches the fetched cid?
        |> (decryptBlock cipher key) // returns PlainContent
    | Some(data) -> data


let encryptPut (putRaw: PutRawType) (storeCid: StoreCidType)
    (calcPlainCid: CalcPlainCidType) // TODO: asymmetry OK here?
    (cache: Cache.PlainDataCache) (cipher: Cipher) (key: byte[])
    (data: PlainContent): PlainContentId =

    let plainCid = calcPlainCid data // we can't pass on plaintext to the IPFS
    Cache.putIntoCache cache plainCid data // returns unit
    encryptBlock cipher key data // returns EncryptedContent
    |> putRaw // returns EncryptedContentId
    |> (storeCid cipher key) plainCid // returns unit
    plainCid // TODO: hash here to verify the cid returned by the underlying IPFS node?
