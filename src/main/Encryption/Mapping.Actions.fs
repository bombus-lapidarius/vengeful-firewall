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
    continuation
    (root: Root)
    (newValue: DagNodeRef)
    (oldValue: DagNodeRef)
    index
    value
    : bool =

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
    | true -> true
    | false ->
        continuation
            root
            resultingValue
            index
            value


// for convenience

type ParseShardBlock1Type = PlainContent -> ShardingKind * PlainContent
type ParseShardBlock2Type = ShardingKind -> PlainContent -> IStoreShard


// "outer" parser: determine the sharding algorithm in order to call the correct
// "inner" parser at runtime
let private parse1 () = ()
// "inner" parser: call the appropriate constructor or parser method
let private parse2 () = ()


type private LeafOpCoreArgs<'T> = {
    ParentCollection: Generic.Stack<IStoreShard>
    CurrentShard: IStoreShard
    MappingPairIndex: PlainContentId
    MappingPairValue: 'T
}

type private LeafOpType<'T> =
    DagNodeRef option -> LeafOpCoreArgs<'T> -> DagNodeRef option


// this one is identical for all operations over the store
let rec private lookupHelper<'Value>
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
    (hookCollection: HookCollection)
    (leafOp: LeafOpType<'Value>)
    (parentCollection: Generic.Stack<IStoreShard>)
    (encryptedShardId: DagNodeRef)
    (index: PlainContentId)
    (value: 'Value)
    : DagNodeRef option =

    // fetch and decrypt the current shard
    let currentShardShardingKind, currentShardRawContent =
        encryptedShardId.Target
        |> hookCollection.GetBlock
        |> hookCollection.DecryptBlock encryptedShardId.Cipher
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
        leafOp
            result
            {
                ParentCollection = parentCollection
                CurrentShard = currentShard
                MappingPairIndex = index
                MappingPairValue = value
            }
    | Tree (substructure) ->
        parentCollection.Push currentShard
        lookupHelper<'Value>
            parseShardBlock1
            parseShardBlock2
            hookCollection
            leafOp
            parentCollection
            substructure
            index
            value


[<CompiledName("Lookup")>]
let lookup
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
    (hookCollection: HookCollection)
    (root: Root)
    (index: PlainContentId)
    //(value: DagNodeRef)
    : DagNodeRef option =

    let currentRootValue = root.TopLevelNode

    // recurse
    lookupHelper<unit>
        parseShardBlock1
        parseShardBlock2
        hookCollection
        (fun result _ -> result)
        (Generic.Stack<IStoreShard>())
        currentRootValue
        index
        ()


let rec private modifyHelper
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
    (hookCollection: HookCollection)
    (modify: LeafOpType<DagNodeRef>)
    (root: Root)
    (currentDagNode: DagNodeRef)
    (index: PlainContentId)
    (value: DagNodeRef)
    : bool =

    // Apply our modification and rebalance the data structure if necessary.
    let intermediateResult =
        lookupHelper<DagNodeRef>
            parseShardBlock1
            parseShardBlock2
            hookCollection
            modify
            (Generic.Stack<IStoreShard>())
            currentDagNode
            index
            value

    // Simplify the recursor function using partial application.
    let recursor =
        modifyHelper
            parseShardBlock1
            parseShardBlock2
            hookCollection
            modify

    // Attempt to replace the current root node. Retry the procedure in case
    // of failure.
    match intermediateResult with
    | Some (result) ->
        updateRootAtomically
            recursor
            root
            result
            currentDagNode
            index
            value
    | None -> false


[<CompiledName("Insert")>]
let insert
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
    (hookCollection: HookCollection)
    (root: Root)
    (index: PlainContentId)
    (value: DagNodeRef)
    : bool =

    let modifyLeaf result (coreArgs: LeafOpCoreArgs<DagNodeRef>) =
        match result with
        | None ->
            coreArgs.CurrentShard.Insert
                hookCollection
                coreArgs.ParentCollection
                coreArgs.MappingPairIndex
                coreArgs.MappingPairValue
        | Some (_) ->
            raise (
                System.ArgumentException
                    "the specified key already exists"
            )

    let currentRootValue = root.TopLevelNode

    modifyHelper
        parseShardBlock1
        parseShardBlock2
        hookCollection
        modifyLeaf
        root
        currentRootValue
        index
        value


//InsertOrUpdate


[<CompiledName("Update")>]
let update
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
    (hookCollection: HookCollection)
    (root: Root)
    (index: PlainContentId)
    (value: DagNodeRef)
    : bool =

    let modifyLeaf result (coreArgs: LeafOpCoreArgs<DagNodeRef>) =
        match result with
        | Some (_) ->
            coreArgs.CurrentShard.Update
                hookCollection
                coreArgs.ParentCollection
                coreArgs.MappingPairIndex
                coreArgs.MappingPairValue
        | None ->
            raise (
                Generic.KeyNotFoundException
                    "no entry for the specified key found"
            )

    let currentRootValue = root.TopLevelNode

    modifyHelper
        parseShardBlock1
        parseShardBlock2
        hookCollection
        modifyLeaf
        root
        currentRootValue
        index
        value


[<CompiledName("Delete")>]
let delete
    (parseShardBlock1: ParseShardBlock1Type)
    (parseShardBlock2: ParseShardBlock2Type)
    (hookCollection: HookCollection)
    (root: Root)
    (index: PlainContentId)
    (value: DagNodeRef)
    : bool =

    let modifyLeaf result (coreArgs: LeafOpCoreArgs<DagNodeRef>) =
        match result with
        | Some (_) ->
            coreArgs.CurrentShard.Delete
                hookCollection
                coreArgs.ParentCollection
                coreArgs.MappingPairIndex
                coreArgs.MappingPairValue
        | None ->
            raise (
                Generic.KeyNotFoundException
                    "no entry for the specified key found"
            )

    let currentRootValue = root.TopLevelNode

    modifyHelper
        parseShardBlock1
        parseShardBlock2
        hookCollection
        modifyLeaf
        root
        currentRootValue
        index
        value


//DeleteIfExists
