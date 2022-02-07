module VengefulFi.Encryption.Block


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


type GenericContentId = Types.GenericContentId
type GenericContent = Types.GenericContent


type PlainContentId = Types.PlainContentId
type PlainContent = Types.PlainContent


type EncryptedContentId = Types.EncryptedContentId
type EncryptedContent = Types.EncryptedContent


type Cipher = Types.Cipher


type CalculateCidType = GenericContent -> GenericContentId


// parameterise based on these function types to enable efficient unit testing
type FetchCidType = Cipher -> byte[] -> PlainContentId -> EncryptedContentId
type StoreCidType = Cipher -> byte[] -> PlainContentId -> EncryptedContentId -> unit


// parameterise based on these function types to enable efficient unit testing
type GetRawType = EncryptedContentId -> EncryptedContent
type PutRawType = EncryptedContent -> EncryptedContentId


exception IllegalKeySizeException of Cipher * byte[]
exception IllegalIVectorSizeException of Cipher * byte[]


let private essiv cipher (key: byte array) (blockId: uint64): byte array =
    // linux dmcrypt ensures that the block number is written to memory as
    // a little endian 64 bits integer before it is used for essiv
    let toLittleEndian (x: byte array) =
        match System.BitConverter.IsLittleEndian with
        | true -> x
        | false -> Array.rev x // TODO: what about mixed endian systems?
    match cipher with
    | Types.Cipher.AesCbc ->
        // create the crypto engine that we'll use for encrypting the block id
        use ecb = Aes.Create()
        // only support sha256 for essiv for now
        use sha256 = SHA256.Create()
        // verify that the key size is valid for the chosen cipher
        match sha256.ComputeHash(key).Length * 8 |> ecb.ValidKeySize with
        | true -> ()
        | false -> raise (IllegalKeySizeException (cipher, key))
        // set the key
        ecb.Key <- sha256.ComputeHash(key)
        // ...
        ecb.Mode <- CipherMode.ECB
        // TODO: set a dummy iv for ecb just in case?
        // ...
        ecb.Padding <- PaddingMode.None // NOTE: this may be incorrect
        // TODO: set block and feedback sizes?
        // ...
        let blockIdLittleEndian = System.BitConverter.GetBytes(blockId) |> toLittleEndian
        let filler x =
            if x < blockIdLittleEndian.Length // clone blockIdLittleEndian first
                then blockIdLittleEndian.[x]
                else 0uy // fill the remainder with zeroes
        let blockIdPadded = Array.init 16 filler
        use ecbEncryptor = ecb.CreateEncryptor()
        // NOTE: this may be incorrect
        let out = ecbEncryptor.TransformFinalBlock(blockIdPadded, 0, 16)
        // make sure that we got an iv of valid size for the chosen cipher
        match out.Length * 8 with
        | 128 -> out
        | _ -> raise (IllegalIVectorSizeException (cipher, out))


let private createCryptoEngine (cipher: Cipher) (key: byte[]) (blockId: uint64): Aes = // TODO: generalise
    match cipher with
    | Types.Cipher.AesCbc ->
        // create the crypto engine that this function will return
        let aes = Aes.Create()
        // verify that the key size is valid for the chosen cipher
        match key.Length * 8 |> aes.ValidKeySize with
        | true -> ()
        | false -> raise (IllegalKeySizeException (cipher, key))
        // set the key
        aes.Key <- key
        // ...
        aes.Mode <- CipherMode.CBC
        aes.IV <- essiv cipher key blockId
        // ...
        aes.Padding <- PaddingMode.PKCS7 // set this explicitly just in case
        // TODO: set block and feedback sizes?
        aes // cbc


let decryptBlock (cipher: Cipher) (key: byte[]) (input: EncryptedContent):
    PlainContent =

    use aes = createCryptoEngine cipher key 0x00uL // TODO: dangling ref?
    let cbcDecryptor = aes.CreateDecryptor()
    let (Types.EncryptedContent (Types.GenericContent inputStream)) = input
    let output = new CryptoStream(inputStream, cbcDecryptor, CryptoStreamMode.Read, false) // TODO: does this return a usable stream object?
    Types.PlainContent (Types.GenericContent output)


let encryptBlock (cipher: Cipher) (key: byte[]) (input: PlainContent):
    EncryptedContent =

    use aes = createCryptoEngine cipher key 0x00uL // TODO: dangling ref?
    let cbcEncryptor = aes.CreateEncryptor()
    let (Types.PlainContent (Types.GenericContent inputStream)) = input
    let output = new CryptoStream(inputStream, cbcEncryptor, CryptoStreamMode.Read, false) // TODO: does this return a usable stream object?
    Types.EncryptedContent (Types.GenericContent output)


let decryptGet (getRaw: GetRawType) (fetchCid: FetchCidType)
    (cache: Cache.PlainDataCache) (cipher: Cipher) (key: byte[])
    (plainCid: PlainContentId): PlainContent =

    match Cache.getFromCache cache plainCid with // PlainContent
    | None -> // we need to fetch the data from IPFS
        fetchCid cipher key plainCid // returns EncryptedContentId
        |> getRaw // returns EncryptedContent
        // TODO: hash here to verify that the encrypted content returned matches the fetched cid?
        |> (decryptBlock cipher key) // returns PlainContent
    | Some(result) -> result


let encryptPut (putRaw: PutRawType) (storeCid: StoreCidType)
    (calculateCid: CalculateCidType) // TODO: asymmetry OK here?
    (cache: Cache.PlainDataCache) (cipher: Cipher) (key: byte[])
    (plainContent: PlainContent): PlainContentId =

    // we can't pass on plaintext to our IPFS node
    // instead, we have to calculate the plaintext cid ourselves
    let (Types.PlainContent input) = plainContent // deconstruct
    let plainCid = Types.PlainContentId (calculateCid input)
    Cache.putIntoCache cache plainCid plainContent // returns unit
    encryptBlock cipher key plainContent // returns EncryptedContent
    |> putRaw // returns EncryptedContentId
    |> (storeCid cipher key) plainCid // returns unit
    plainCid // TODO: hash here to verify the cid returned by the underlying IPFS node?
