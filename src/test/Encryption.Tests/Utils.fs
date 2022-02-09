module VengefulFi.Encryption.Tests.Utils


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
open System.Security.Cryptography


open VengefulFi.Encryption.Types
open VengefulFi.Encryption.Block


type CidMapping =
    { PlainCid: PlainContentId
      EncryptedCid: EncryptedContentId }

type KeyMapping =
    { AesKey: byte []
      Mappings: list<CidMapping> }


exception Base64ConversionFailedException of string
exception HexStrConversionFailedException of string


exception UnknownContentIdException
exception UnknownContentException


exception IllegalContentIdException
exception IllegalContentException


// move data from generic dotnet stream objects to byte arrays
let fromStream (rawStream: Stream) : byte [] =
    use ms = new MemoryStream(256)
    rawStream.CopyTo(ms) // this should adjust the MemoryStream size as needed
    ms.ToArray()
// move data from byte arrays to generic dotnet stream objects
let toStream (rb: byte []) : Stream = new MemoryStream(rb) :> Stream // upcast


// compare two byte arrays by folding them using a function that tests their
// individual bytes for equality
let compareByteArray (x: byte array) (y: byte array) =
    let fd s a b =
        if a = b // bytes at identical index positions must be equal
        then
            s
        else
            s + 1

    try
        match Array.fold2 fd 0 x y with
        | 0 -> true
        | _ -> false
    with
    // two arrays of different length cannot be the same
    | :? System.ArgumentException -> false


// compare two cids by extracting and comparing their underlying byte arrays
let comparePlainCids x y : bool =
    let (PlainContentId (GenericContentId a)) = x
    let (PlainContentId (GenericContentId b)) = y
    compareByteArray a b
// compare two cids by extracting and comparing their underlying byte arrays
let compareEncryptedCids x y : bool =
    let (EncryptedContentId (GenericContentId a)) = x
    let (EncryptedContentId (GenericContentId b)) = y
    compareByteArray a b


// encoding conversions (base64)
let fromBase64 s =
    try
        System.Convert.FromBase64String(s)
    with
    // the string may contain illegal characters, pass on the offending string
    | _ -> raise (Base64ConversionFailedException s)

let toBase64 r = System.Convert.ToBase64String(r)


// encoding conversions (hexstr)
let fromHexStr (s: string) =
    try
        System.Convert.FromHexString(s)
    with
    // the string may contain illegal characters, pass on the offending string
    | _ -> raise (HexStrConversionFailedException s)

let toHexStr (r: byte array) = System.Convert.ToHexString(r)


// cid encoding conversions (base64)
let plainCidfromBase64 s =
    fromBase64 s |> GenericContentId |> PlainContentId
// cid encoding conversions (base64)
let encryptedCidfromBase64 s =
    fromBase64 s
    |> GenericContentId
    |> EncryptedContentId
// cid encoding conversions (base64)
let plainCidtoBase64 i =
    let (PlainContentId (GenericContentId r)) = i
    toBase64 r
// cid encoding conversions (base64)
let encryptedCidtoBase64 i =
    let (EncryptedContentId (GenericContentId r)) = i
    toBase64 r


// cid encoding conversions (hexstr)
let plainCidfromHexStr s =
    fromHexStr s |> GenericContentId |> PlainContentId
// cid encoding conversions (hexstr)
let encryptedCidfromHexStr s =
    fromHexStr s
    |> GenericContentId
    |> EncryptedContentId
// cid encoding conversions (hexstr)
let plainCidtoHexStr i =
    let (PlainContentId (GenericContentId r)) = i
    toHexStr r
// cid encoding conversions (hexstr)
let encryptedCidtoHexStr i =
    let (EncryptedContentId (GenericContentId r)) = i
    toHexStr r


let private keyAndCidMappingFromString (s: string) =
    let a = s.Split([| '.' |])

    let aesKey = fromHexStr a.[0]
    let plainCid = plainCidfromHexStr a.[1]
    let encryptedCid = encryptedCidfromHexStr a.[2]

    (aesKey,
     { PlainCid = plainCid
       EncryptedCid = encryptedCid })


let getKeyMappingsFromStrings (l: list<string>) =
    let rawMappingList = List.map keyAndCidMappingFromString l

    let keys = List.map fst rawMappingList

    let extractCidMappingsForKey l k =
        List.filter (fun e -> compareByteArray (fst e) k) l
        |> List.map snd

    List.map
        (fun x ->
            { AesKey = x
              Mappings = (extractCidMappingsForKey rawMappingList) x })
        keys


let fetchCidMock (l: list<KeyMapping>) cipher key (plainCid: PlainContentId) =
    let f = compareByteArray
    let g = comparePlainCids
    // search our test mappings for the correct content id
    try
        let a = List.find (fun x -> f x.AesKey key) l
        let b = List.find (fun y -> g y.PlainCid plainCid) a.Mappings
        b.EncryptedCid
    with
    | _ -> raise UnknownContentIdException


// this need not do anything
let storeCidMock (l: list<KeyMapping>) cipher key plainCid encryptedCid = ()


let putRawMock s c : EncryptedContentId =
    // deconstruct
    let (EncryptedContent (GenericContent stream)) = c
    // generate the correct hash for the data provided
    use hasher = SHA256.Create()
    hasher.ComputeHash(stream)
    // construct
    |> GenericContentId
    |> EncryptedContentId


// for testing purposes, reuse existing code
let putGenericMock s c : GenericContentId =
    // deconstruct
    let (EncryptedContentId i) = putRawMock s (EncryptedContent c)
    i

let putPlainMock s c : PlainContentId =
    // deconstruct
    let (PlainContent g) = c
    putGenericMock s g |> PlainContentId


let getRawMock i : EncryptedContent =
    let assembly = System.Reflection.Assembly.GetExecutingAssembly()
    // retrieve the correct data for the hash provided
    try
        let filename =
            "VengefulFi.Encryption.Tests.res."
            + (encryptedCidtoHexStr i).ToLower()
            + ".raw"

        let stream = assembly.GetManifestResourceStream(filename)

        if stream = null then
            NUnit.Framework.TestContext.WriteLine(
                "embedded file "
                + filename
                + " could not be loaded"
            )

            raise UnknownContentException
        else
            stream
    with
    // embedded file not found
    | :? FileNotFoundException -> raise UnknownContentException
    | :? System.ArgumentException -> raise IllegalContentIdException
    // found but malformed somehow
    | :? FileLoadException -> raise IllegalContentException
    | :? System.BadImageFormatException -> raise IllegalContentException
    // construct
    |> GenericContent
    |> EncryptedContent


// for testing purposes, reuse existing code
let getGenericMock i : GenericContent =
    // deconstruct
    let (EncryptedContent c) = getRawMock (EncryptedContentId i)
    c

let getPlainMock i : PlainContent =
    // deconstruct
    let (PlainContentId g) = i
    getGenericMock g |> PlainContent
