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


type private KVPairType = Generic.KeyValuePair<string, DataModel.INode>


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

        member this.AsUInt64 = backingStorage.TargetSize

        member this.IsNull = false


[<ProtoContract>]
[<ProtoInclude(63, "PBNodeRepr")>]
type PBLink(hash: RawContentId, name: string, targetSize: uint64) =
    inherit DataModel.Base()

    // deconstruct, this shouldn't require any backing storage
    let (RawContentId bareHash) = hash

    // provide the child nodes with a handle to our data
    let mutable hashNodeRepr: PBLinkHash option = None
    let mutable nameNodeRepr: PBLinkName option = None
    let mutable targetSizeNodeRepr: PBLinkTargetSize option = None

    // use the protobuf representation to store our data

    [<ProtoMember(1)>]
    member val HashRepr: byte array = bareHash with get, set

    [<ProtoMember(2)>]
    member val NameRepr: string = name with get, set

    [<ProtoMember(3)>]
    member val TargetSizeRepr: uint64 = targetSize with get, set

    // this shall only be used by our factory method
    member this.HashNode
        with private get () = hashNodeRepr
        and private set (value) = hashNodeRepr <- value

    // this shall only be used by our factory method
    member this.NameNode
        with private get () = nameNodeRepr
        and private set (value) = nameNodeRepr <- value

    // this shall only be used by our factory method
    member this.TargetSizeNode
        with private get () = targetSizeNodeRepr
        and private set (value) = targetSizeNodeRepr <- value

    static member Create(hash: RawContentId, name, targetSize, deepCopy: bool) =
        // deconstruct, this shouldn't require any backing storage
        let (RawContentId bareHash) = hash

        let hashRepr =
            if deepCopy then
                bareHash.ToArray() |> RawContentId
            else
                bareHash |> RawContentId

        let initPBLink = PBLink(hashRepr, name, targetSize)

        let initPBLinkHash = PBLinkHash(initPBLink)
        let initPBLinkName = PBLinkName(initPBLink)
        let initPBLinkTargetSize = PBLinkTargetSize(initPBLink)

        // provide the child nodes with a handle to our data
        initPBLink.HashNode <- Some(initPBLinkHash)
        initPBLink.NameNode <- Some(initPBLinkName)
        initPBLink.TargetSizeNode <- Some(initPBLinkTargetSize)

        initPBLink

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

        member this.AsDictionary =
            // return an ad-hoc implementation
            { new Generic.IReadOnlyDictionary<string, DataModel.INode> with
                member self.TryGetValue(strKey, n: byref<DataModel.INode>) =
                    match strKey with
                    | "Hash" ->
                        match this.HashNode with
                        | Some (node) ->
                            n <- (node :> DataModel.INode)
                            true
                        | None -> raise DataModel.NotInitialisedException
                    | "Name" ->
                        match this.NameNode with
                        | Some (node) ->
                            n <- (node :> DataModel.INode)
                            true
                        | None -> raise DataModel.NotInitialisedException
                    | "TargetSize" ->
                        match this.TargetSizeNode with
                        | Some (node) ->
                            n <- (node :> DataModel.INode)
                            true
                        | None -> raise DataModel.NotInitialisedException
                    | _ ->
                        n <- (DataModel.Trivial.Null() :> DataModel.INode)
                        false

                member self.get_Item(key: string) =
                    match self.TryGetValue(key) with
                    | true, node -> node
                    | _ -> raise (Generic.KeyNotFoundException "TODO")

                member self.get_Keys() = [ "Hash"; "Name"; "TargetSize" ] :> seq<string>

                member self.get_Values() =
                    self.get_Keys ()
                    |> Seq.map (fun key -> self.get_Item (key))

                member self.get_Count() = 3 // hard-wire this

                member self.ContainsKey(key: string) =
                    self.get_Keys ()
                    |> List.ofSeq
                    |> List.contains key

                member self.GetEnumerator() : Generic.IEnumerator<KVPairType> =
                    let mappingFun key = KVPairType(key, self.get_Item(key))
                    let keys = self.get_Keys()
                    let collection = Seq.map mappingFun keys

                    collection.GetEnumerator()

                member self.GetEnumerator() : IEnumerator =
                    let mappingFun key = KVPairType(key, self.get_Item(key))
                    let keys = self.get_Keys()
                    let collection = Seq.map mappingFun keys

                    collection.GetEnumerator() :> System.Collections.IEnumerator }


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
type PBNode(data: byte array, linkList: Generic.List<PBLink>) =
    inherit DataModel.Base()

    // provide the child nodes with a handle to our data
    let mutable dataNodeRepr: PBNodeData option = None
    let mutable linkListNodeRepr: PBNodeLinkList option = None

    // use the protobuf representation to store our data

    [<ProtoMember(1)>]
    member val DataRepr: byte array = data with get, set

    [<ProtoMember(2)>]
    member val LinkListRepr: Generic.List<PBLink> = linkList with get, set

    // this shall only be used by our factory method
    member this.DataNode
        with private get () = dataNodeRepr
        and private set (value) = dataNodeRepr <- value

    // this shall only be used by our factory method
    member this.LinkListNode
        with private get () = linkListNodeRepr
        and private set (value) = linkListNodeRepr <- value

    static member Create(data: byte array, linkList, deepCopy: bool) =
        let dataRepr =
            if deepCopy then
                data.ToArray() // copies the individual elements
            else
                data

        let linkListRepr: Generic.List<PBLink> =
            if deepCopy then
                let linkListReprInit = Generic.List<PBLink>()

                // populate the generic dotnet list type using imperative code
                for item: PBLink in linkList do
                    let hash = item.HashRepr |> RawContentId
                    let name = item.NameRepr
                    let targetSize = item.TargetSizeRepr

                    linkListReprInit.Add(
                        // deep copy
                        PBLink.Create(hash, name, targetSize, true)
                    )

                linkListReprInit
            else
                linkList

        let initPBNode = PBNode(dataRepr, linkListRepr)

        let initPBNodeData = PBNodeData(initPBNode)
        let initPBNodeLinkList = PBNodeLinkList(initPBNode)

        // provide the child nodes with a handle to our data
        initPBNode.DataNode <- Some(initPBNodeData)
        initPBNode.LinkListNode <- Some(initPBNodeLinkList)

        initPBNode

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

        member this.AsDictionary =
            // return an ad-hoc implementation
            { new Generic.IReadOnlyDictionary<string, DataModel.INode> with
                member self.TryGetValue(strKey, n: byref<DataModel.INode>) =
                    match strKey with
                    | "Data" ->
                        match this.DataNode with
                        | Some (node) ->
                            n <- (node :> DataModel.INode)
                            true
                        | None -> raise DataModel.NotInitialisedException
                    | "LinkList" ->
                        match this.LinkListNode with
                        | Some (node) ->
                            n <- (node :> DataModel.INode)
                            true
                        | None -> raise DataModel.NotInitialisedException
                    | _ ->
                        n <- (DataModel.Trivial.Null() :> DataModel.INode)
                        false

                member self.get_Item(key: string) =
                    match self.TryGetValue(key) with
                    | true, node -> node
                    | _ -> raise (Generic.KeyNotFoundException "TODO")

                member self.get_Keys() = [ "Data"; "LinkList" ] :> seq<string>

                member self.get_Values() =
                    self.get_Keys ()
                    |> Seq.map (fun key -> self.get_Item (key))

                member self.get_Count() = 2 // hard-wire this

                member self.ContainsKey(key: string) =
                    self.get_Keys ()
                    |> List.ofSeq
                    |> List.contains key

                member self.GetEnumerator() : Generic.IEnumerator<KVPairType> =
                    let mappingFun key = KVPairType(key, self.get_Item(key))
                    let keys = self.get_Keys()
                    let collection = Seq.map mappingFun keys

                    collection.GetEnumerator()

                member self.GetEnumerator() : IEnumerator =
                    let mappingFun key = KVPairType(key, self.get_Item(key))
                    let keys = self.get_Keys()
                    let collection = Seq.map mappingFun keys

                    collection.GetEnumerator() :> System.Collections.IEnumerator }
