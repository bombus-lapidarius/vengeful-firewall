module VengefulFi.Ipld.Convert


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


open WojciechMikołajewicz


open VengefulFi.Ipld


exception VarintIncompleteException
exception VarintOverflowException


exception MalformedBinaryCidException


exception Base64ConversionFailedException of string
exception HexStrConversionFailedException of string


// provide the third party library with a functional shell

let private uInt32FromVarint (a: byte []) : uint32 * int =
    let s = new System.ReadOnlySpan<byte>(a)

    try
        match Base128.TryReadUInt32(s) with
        | true, output, amount -> output, amount
        | false, _, _ -> raise VarintIncompleteException
    with
    | :? System.OverflowException -> raise VarintOverflowException

let private uInt32toVarint (u: uint32) : byte [] =
    let mutable a = Array.zeroCreate<byte> 5 // should always suffice for uint32
    let s = new System.Span<byte>(a)

    match Base128.TryWriteUInt32(s, u) with
    | true, n -> Array.sub a 0 n
    | false, _ -> raise VarintOverflowException


let private sliceOffVarintAsUInt32 (a: byte []) : uint32 * byte [] =
    let varint, amount = uInt32FromVarint a
    let remainingArray = Array.sub a amount (a.Length - amount)

    varint, remainingArray


let parseBinaryCid (rawContentId: RawContentId) : GenericContentIdFuture =
    let (RawContentId forward0) = rawContentId

    let cidVersion, forward1 = sliceOffVarintAsUInt32 forward0
    let multicodec, forward2 = sliceOffVarintAsUInt32 forward1
    let hashName, forward3 = sliceOffVarintAsUInt32 forward2
    let hashSize, forward4 = sliceOffVarintAsUInt32 forward3

    // make sure the digest is of correct size
    let digest =
        if (uint32 forward4.Length) = hashSize then
            forward4
        else
            raise MalformedBinaryCidException

    { CidVersion = LanguagePrimitives.EnumOfValue cidVersion
      Multicodec = LanguagePrimitives.EnumOfValue multicodec
      Hash =
        { Name = LanguagePrimitives.EnumOfValue hashName
          Size = hashSize
          Digest = digest |> Digest } }


let composeBinaryCid (genericContentId: GenericContentIdFuture) : RawContentId =
    let (Digest digest) = genericContentId.Hash.Digest

    [ uint32 genericContentId.CidVersion
      |> uInt32toVarint
      uint32 genericContentId.Multicodec
      |> uInt32toVarint
      uint32 genericContentId.Hash.Name
      |> uInt32toVarint
      uint32 genericContentId.Hash.Size
      |> uInt32toVarint
      digest ]
    |> Array.concat
    |> RawContentId


// move data from generic dotnet stream objects to byte arrays
let fromStream (rawStream: Stream) : byte [] =
    use ms = new MemoryStream(256)
    rawStream.CopyTo(ms) // this should adjust the MemoryStream size as needed
    ms.ToArray()

// move data from byte arrays to generic dotnet stream objects
let toStream (rb: byte []) : Stream = new MemoryStream(rb) :> Stream // upcast


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
