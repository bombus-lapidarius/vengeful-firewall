module VengefulFi.Encryption.Mapping.Actions


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
open System.Threading


open VengefulFi.Encryption


let private updateRootAtomically
    (root: Root)
    (newValue: DagNodeRef)
    (oldValue: DagNodeRef) =

    // At this point, we always hold a reference to oldValue; which means that
    // the garbage collector should leave it alone. That in turn means that as
    // long as our root record still contains a reference to this oldValue
    // (which itself ought to be immutable and only hold references to other
    // immutable data), we may assume that no other thread has inserted an
    // updated root node into our dag.
    // Therefore, a successful compare-and-swap operation means that we have
    // successfully updated the root node ourselves.
    let resultingValue =
        Interlocked.CompareExchange<DagNodeRef>
            (&(root.TopLevelNode), newValue, oldValue)

    match resultingValue = oldValue with
    | true -> (true, newValue)
    | false -> (false, resultingValue)


// for convenience

type ParseShardBlock1Type = PlainContent -> ShardingKind * PlainContent
type ParseShardBlock2Type = ShardingKind -> PlainContent -> IStoreShard


// "outer" parser: determine the sharding algorithm in order to call the correct
// "inner" parser at runtime
let private parse1 () = ()
// "inner" parser: call the appropriate constructor or parser method
let private parse2 () = ()


type private LeafOpCoreArgs<'T> =
    Generic.Stack<IStoreShard> * IStoreShard * PlainContentId * 'T
type private LeafOpType<'T> =
    DagNodeRef option -> LeafOpCoreArgs<'T> -> DagNodeRef option


// this one is identical for all operations over the store
let rec private lookupHelper<'Value>
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
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
        encryptedShardId.Target
        |> getBlock
        |> decryptBlock encryptedShardId.Cipher
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
    | Tree (substructure) ->
        parentCollection.Push currentShard
        lookupHelper<'Value>
            parseShardBlock1
            parseShardBlock2
            getBlock
            decryptBlock
            leafOp
            parentCollection
            substructure
            index
            value


[<CompiledName("Lookup")>]
let lookup
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
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
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
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


//InsertOrUpdate


[<CompiledName("Update")>]
let update
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
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
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
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


//DeleteIfExists
