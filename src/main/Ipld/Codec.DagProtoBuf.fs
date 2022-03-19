module VengefulFi.Ipld.Codec.DagProtoBuf


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


open System.Collections
open System.Linq


open ProtoBuf


open VengefulFi.Ipld


// TODO: optional (see dag-pb spec)
// TODO: repeated -> List<PBLink> OK?


type private IPBLinkInit =
    abstract member Hash: byte array with get, set
    abstract member Name: string with get, set
    abstract member TargetSize: uint64 with get, set


type private PBLinkHash(backingStorage: IPBLinkInit) =
    inherit DataModel.Base()

    member this.Update(newValue: RawContentId) =
        let (RawContentId bareHash) = newValue

        backingStorage.Hash <- bareHash

    member this.Update(newValue: GenericContentIdFuture) =
        let (RawContentId bareHash) = newValue |> Convert.composeBinaryCid

        backingStorage.Hash <- bareHash

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Link

        member this.AsGenericContentId =
            backingStorage.Hash
            |> RawContentId
            |> Convert.parseBinaryCid

        member this.AsRawContentId = backingStorage.Hash |> RawContentId

        member this.IsNull = false

type private PBLinkName(backingStorage: IPBLinkInit) =
    inherit DataModel.Base()

    member this.Update(newValue: string) = backingStorage.Name <- newValue

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.String

        member this.AsString = backingStorage.Name

        member this.IsNull = false

type private PBLinkTargetSize(backingStorage: IPBLinkInit) =
    inherit DataModel.Base()

    member this.Update(newValue: uint64) = backingStorage.TargetSize <- newValue

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Integer

        member this.AsInteger = bigint backingStorage.TargetSize // TODO

        member this.IsNull = false


[<ProtoContract>]
[<ProtoInclude(63, "PBNodeRepr")>]
type PBLink(hash: RawContentId, name: string, targetSize: uint64) as this =
    inherit DataModel.Base()

    // deconstruct, this shouldn't require any backing storage
    let (RawContentId bareHash) = hash

    // provide the child nodes with a handle to our data
    let hashNode = PBLinkHash(this :> IPBLinkInit)
    let nameNode = PBLinkName(this :> IPBLinkInit)
    let targetSizeNode = PBLinkTargetSize(this :> IPBLinkInit)

    // use the protobuf representation to store our data

    [<ProtoMember(1)>]
    member val HashRepr: byte array = bareHash with get, set

    [<ProtoMember(2)>]
    member val NameRepr: string = name with get, set

    [<ProtoMember(3)>]
    member val TargetSizeRepr: uint64 = targetSize with get, set

    interface IPBLinkInit with
        member this.Hash
            with get () = this.HashRepr
            and set (value) = this.HashRepr <- value

        member this.Name
            with get () = this.NameRepr
            and set (value) = this.NameRepr <- value

        member this.TargetSize
            with get () = this.TargetSizeRepr
            and set (value) = this.TargetSizeRepr <- value

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Map

        member this.IsNull = false

        member this.LookupByString s =
            match s with
            | "Hash" -> hashNode :> DataModel.INode
            | "Name" -> nameNode :> DataModel.INode
            | "TargetSize" -> targetSizeNode :> DataModel.INode
            | _ -> DataModel.Trivial.Null() :> DataModel.INode


type private IPBNodeInit =
    abstract member Data: byte array with get, set
    abstract member LinkList: Generic.List<PBLink> with get, set


type private PBNodeData(backingStorage: IPBNodeInit) =
    inherit DataModel.Base()

    member this.Update(newValue: RawContent) =
        let a = newValue |> Convert.fromStream

        backingStorage.Data <- a

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Bytes

        member this.AsRawContent = backingStorage.Data |> Convert.toStream

        member this.IsNull = false


type private PBNodeLinkList(backingStorage: IPBNodeInit) =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.List

        // TODO: list

        member this.IsNull = false

// TODO: lookup


[<ProtoContract>]
type PBNode(data: byte array, linkList: Generic.List<PBLink>, deepCopy: bool) as this =
    inherit DataModel.Base()

    let dataRepr =
        if deepCopy then
            data.ToArray() // copies the individual elements
        else
            data

    let linkListRepr =
        if deepCopy then
            let linkListReprInit = Generic.List<PBLink>()

            // populate the generic dotnet list type using imperative code
            for item in linkList do
                // TODO: deep copy the individual elements too
                linkListReprInit.Add(item)

            linkListReprInit
        else
            linkList

    // provide the child nodes with a handle to our data
    let dataNode = PBNodeData(this :> IPBNodeInit)
    let linkListNode = PBNodeLinkList(this :> IPBNodeInit)

    // use the protobuf representation to store our data

    [<ProtoMember(1)>]
    member val DataRepr: byte array = dataRepr with get, set

    [<ProtoMember(2)>]
    member val LinkListRepr: Generic.List<PBLink> = linkListRepr with get, set

    interface IPBNodeInit with
        member this.Data
            with get () = this.DataRepr
            and set (value) = this.DataRepr <- value

        member this.LinkList
            with get () = this.LinkListRepr
            and set (value) = this.LinkListRepr <- value

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Map

        member this.IsNull = false

        member this.LookupByString s =
            match s with
            | "Data" -> dataNode :> DataModel.INode
            | "LinkList" -> linkListNode :> DataModel.INode
            | _ -> DataModel.Trivial.Null() :> DataModel.INode
