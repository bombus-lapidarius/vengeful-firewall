module VengefulFi.Encryption.Mapping


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


// for convenience

type private DagNodeRef = byte [] * EncryptedContentId

type private DecryptionType = byte [] -> EncryptedContent -> PlainContent
type private EncryptionType = byte [] -> PlainContent -> EncryptedContent


// TODO: extend at run time (using attributes)
type ShardingKind =
    | HAMT = 0x00uL


// shards can either represent leaves (actual data or a dead end) or trees
// (tree in a generalised sense, actually a dag)
type GenericStoreShardValue =
    | Leaf of DagNodeRef option
    | Tree of DagNodeRef


// this is where the actual implementations live (i.e. whether this is
// a search tree, a HAMT etc.)
type IStoreShard =
    abstract member Kind: ShardingKind

    // arg: parentCollection
    // arg: index (or key)
    abstract member Find:
        Generic.Stack<IStoreShard> -> PlainContentId -> GenericStoreShardValue

    // arg: getBlock
    // arg: putBlock
    // arg: decryptBlock
    // arg: encryptBlock
    // arg: parentCollection
    // arg: index (or key)
    // arg: new or updated value
    abstract member Insert:
        (EncryptedContentId -> EncryptedContent) ->
        (EncryptedContent -> EncryptedContentId) ->
        DecryptionType ->
        EncryptionType ->
        Generic.Stack<IStoreShard> ->
        PlainContentId ->
        DagNodeRef ->
            DagNodeRef option

    // arg: getBlock
    // arg: putBlock
    // arg: decryptBlock
    // arg: encryptBlock
    // arg: parentCollection
    // arg: index (or key)
    // arg: new or updated value
    abstract member Update:
        (EncryptedContentId -> EncryptedContent) ->
        (EncryptedContent -> EncryptedContentId) ->
        DecryptionType ->
        EncryptionType ->
        Generic.Stack<IStoreShard> ->
        PlainContentId ->
        DagNodeRef ->
            DagNodeRef option

    // arg: getBlock
    // arg: putBlock
    // arg: decryptBlock
    // arg: encryptBlock
    // arg: parentCollection
    // arg: index (or key)
    // arg: new or updated value
    abstract member Delete:
        (EncryptedContentId -> EncryptedContent) ->
        (EncryptedContent -> EncryptedContentId) ->
        DecryptionType ->
        EncryptionType ->
        Generic.Stack<IStoreShard> ->
        PlainContentId ->
        DagNodeRef ->
            DagNodeRef option

    //InsertOrUpdate
    //DeleteIfExists

    // TODO
    abstract member DeserializeShard: PlainContent -> IStoreShard
    abstract member SerializeShard: IStoreShard -> PlainContent


// for convenience

type private LeafOpCoreArgs<'T> =
    Generic.Stack<IStoreShard> * IStoreShard * PlainContentId * 'T
type private LeafOpType<'T> =
    DagNodeRef option -> LeafOpCoreArgs<'T> -> DagNodeRef option

// "outer" parser: determine the sharding algorithm in order to call the correct
// "inner" parser at runtime
let private parse1 () = ()
// "inner" parser: call the appropriate constructor or parser method
let private parse2 () = ()


// this one is identical for all operations over the store
let rec private lookupHelper<'Value>
    (parseShardBlock1: PlainContent -> ShardingKind * PlainContent)
    (parseShardBlock2: ShardingKind -> PlainContent -> IStoreShard)
    (getBlock: EncryptedContentId -> EncryptedContent)
    //(putBlock: EncryptedContent -> EncryptedContentId)
    (decryptBlock: DecryptionType)
    //(encryptBlock: EncryptionType)
    (leafOp: LeafOpType<'Value>)
    (parentCollection: Generic.Stack<IStoreShard>)
    (encryptedShardId: DagNodeRef)
    (index: PlainContentId)
    (value: 'Value)
    : DagNodeRef option =

    // fetch and decrypt the current shard
    let currentShardShardingKind, currentShardRawContent =
        (snd encryptedShardId)
        |> getBlock
        |> decryptBlock (fst encryptedShardId)
        |> parseShardBlock1

    // call the correct IStoreShard parser
    let currentShard =
        currentShardRawContent
        |> parseShardBlock2 currentShardShardingKind

    // apply the given function if we encounter a leaf
    // recurse otherwise
    match currentShard.Find parentCollection index with
    | Leaf (result) ->
        // keeping this generic enables us to use the same helper function for
        // both lookup and modification operations
        // only the function arguments the operation in question needs are
        // actually used, all others are discarded
        leafOp result (parentCollection, currentShard, index, value)
    | Tree (key, cid) ->
        parentCollection.Push currentShard
        lookupHelper<'Value>
            parseShardBlock1
            parseShardBlock2
            getBlock
            decryptBlock
            leafOp
            parentCollection
            (key, cid)
            index
            value


[<CompiledName("Lookup")>]
let lookup
    (parseShardBlock1: PlainContent -> ShardingKind * PlainContent)
    (parseShardBlock2: ShardingKind -> PlainContent -> IStoreShard)
    (getBlock: EncryptedContentId -> EncryptedContent)
    //(putBlock: EncryptedContent -> EncryptedContentId)
    (decryptBlock: DecryptionType)
    //(encryptBlock: EncryptionType)
    (shard: DagNodeRef)
    (index: PlainContentId)
    //(value: DagNodeRef)
    : DagNodeRef option =

    // recurse
    lookupHelper<unit>
        parseShardBlock1
        parseShardBlock2
        getBlock
        decryptBlock
        (fun result _ -> result)
        (Generic.Stack<IStoreShard>())
        shard
        index
        ()


[<CompiledName("Insert")>]
let insert
    (parseShardBlock1: PlainContent -> ShardingKind * PlainContent)
    (parseShardBlock2: ShardingKind -> PlainContent -> IStoreShard)
    (getBlock: EncryptedContentId -> EncryptedContent)
    (putBlock: EncryptedContent -> EncryptedContentId)
    (decryptBlock: DecryptionType)
    (encryptBlock: EncryptionType)
    (shard: DagNodeRef)
    (index: PlainContentId)
    (value: DagNodeRef)
    : DagNodeRef option =

    let modify res (parentCollection, currentShard: IStoreShard, k, v) =
        match res with
        | None ->
            currentShard.Insert
                getBlock
                putBlock
                decryptBlock
                encryptBlock
                parentCollection
                k
                v
        | Some (_) ->
            raise (
                System.ArgumentException
                    "the specified key already exists"
            )

    // recurse
    lookupHelper<DagNodeRef>
        parseShardBlock1
        parseShardBlock2
        getBlock
        decryptBlock
        modify
        (Generic.Stack<IStoreShard>())
        shard
        index
        value


[<CompiledName("Update")>]
let update
    (parseShardBlock1: PlainContent -> ShardingKind * PlainContent)
    (parseShardBlock2: ShardingKind -> PlainContent -> IStoreShard)
    (getBlock: EncryptedContentId -> EncryptedContent)
    (putBlock: EncryptedContent -> EncryptedContentId)
    (decryptBlock: DecryptionType)
    (encryptBlock: EncryptionType)
    (shard: DagNodeRef)
    (index: PlainContentId)
    (value: DagNodeRef)
    : DagNodeRef option =

    let modify res (parentCollection, currentShard: IStoreShard, k, v) =
        match res with
        | Some (_) ->
            currentShard.Update
                getBlock
                putBlock
                decryptBlock
                encryptBlock
                parentCollection
                k
                v
        | None ->
            raise (
                Generic.KeyNotFoundException
                    "no entry for the specified key found"
            )

    // recurse
    lookupHelper<DagNodeRef>
        parseShardBlock1
        parseShardBlock2
        getBlock
        decryptBlock
        modify
        (Generic.Stack<IStoreShard>())
        shard
        index
        value


[<CompiledName("Delete")>]
let delete
    (parseShardBlock1: PlainContent -> ShardingKind * PlainContent)
    (parseShardBlock2: ShardingKind -> PlainContent -> IStoreShard)
    (getBlock: EncryptedContentId -> EncryptedContent)
    (putBlock: EncryptedContent -> EncryptedContentId)
    (decryptBlock: DecryptionType)
    (encryptBlock: EncryptionType)
    (shard: DagNodeRef)
    (index: PlainContentId)
    (value: DagNodeRef)
    : DagNodeRef option =

    let modify res (parentCollection, currentShard: IStoreShard, k, v) =
        match res with
        | Some (_) ->
            currentShard.Delete
                getBlock
                putBlock
                decryptBlock
                encryptBlock
                parentCollection
                k
                v
        | None ->
            raise (
                Generic.KeyNotFoundException
                    "no entry for the specified key found"
            )

    // recurse
    lookupHelper<DagNodeRef>
        parseShardBlock1
        parseShardBlock2
        getBlock
        decryptBlock
        modify
        (Generic.Stack<IStoreShard>())
        shard
        index
        value
