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
open System.Linq
open System.Security.Cryptography


open VengefulFi.Ipld


open VengefulFi.Encryption
open VengefulFi.Encryption.Compare
open VengefulFi.Encryption.Convert


type CidMapping =
    { PlainCid: PlainContentId
      EncryptedCid: EncryptedContentId }

type KeyMapping =
    { AesKey: byte []
      Mappings: list<CidMapping> }


let private compareByteArray (x: byte array) (y: byte array) =
    x.SequenceEqual(y)


let private keyAndCidMappingFromString (s: string) =
    let a = s.Split([| '.' |])

    let aesKey = System.Convert.FromHexString a.[0]
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
    |> RawContentId
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
