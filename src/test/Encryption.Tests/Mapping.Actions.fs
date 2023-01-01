module VengefulFi.Encryption.Tests.Mapping.Actions


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


open System.Collections.Immutable
open System.Threading
open System.Linq


open NUnit.Framework


open VengefulFi.Ipld
open VengefulFi.Ipld.Convert


open VengefulFi.Encryption
open VengefulFi.Encryption.Mapping


[<Struct>]
type DagNodeRefMock = DagNodeRefMock of uint64

[<Struct>]
type MappingStoreDagNodeRefMock = MappingStoreDagNodeRefMock of uint64


let private genericContentIdToUInt argument =
    let (RawContentId unwrappedByteArray) = argument
    // Make sure that the byte array can be converted into an integer.
    // TODO: length check
    System.BitConverter.ToUInt64(unwrappedByteArray, 0x00)

let private genericContentToUInt argument =
    let (GenericContent dataStream) = argument
    // Make sure that the byte array can be converted into an integer.
    // TODO: length check
    System.BitConverter.ToUInt64(dataStream |> fromStream, 0x00)

let private plainContentIdToUInt argument =
    let (PlainContentId genericContentId) = argument
    // Delegate the actual work.
    genericContentId |> genericContentIdToUInt

let private plainContentToUInt argument =
    let (PlainContent genericContent) = argument
    // Delegate the actual work.
    genericContent |> genericContentToUInt

let private encryptedContentIdToUInt argument =
    let (EncryptedContentId genericContentId) = argument
    // Delegate the actual work.
    genericContentId |> genericContentIdToUInt

let private encryptedContentToUInt argument =
    let (EncryptedContent genericContent) = argument
    // Delegate the actual work.
    genericContent |> genericContentToUInt


// TODO: adjust when switching from GenericContentId to GenericContentIdFuture
let private fromUInt64 (argument: uint64) =
    System.BitConverter.GetBytes(argument)
    |> RawContentId
    |> EncryptedContentId

let private makeDagNodeRef (target: EncryptedContentId) =
    {
        Cipher = [||]
        Target = target
    }


let private parseInnerMock
    (fg: MappingStoreDagNodeRefMock -> IStoreShard)
    (argument: InnerShard)
    =

    let (InnerShard bare) = argument
    // This mock relies on a mock IStoreShard implementation to do its work.
    bare |> plainContentToUInt |> MappingStoreDagNodeRefMock |> fg

let private parseOuterMock fg (argument: PlainContent) =
    (fg |> parseInnerMock), (argument |> InnerShard)

// TODO: adjust when switching from GenericContentId to GenericContentIdFuture
let private getBlockMock (argument: EncryptedContentId) =
    let (unwrappedValue: uint64) = encryptedContentIdToUInt argument
    System.BitConverter.GetBytes(unwrappedValue)
    |> toStream
    |> GenericContent
    |> EncryptedContent

// TODO: adjust when switching from GenericContentId to GenericContentIdFuture
let private putBlockMock (argument: EncryptedContent) =
    let (unwrappedValue: uint64) = encryptedContentToUInt argument
    // Delegate the actual work.
    unwrappedValue |> fromUInt64

let private decryptBlockMock _ (argument: EncryptedContent) =
    let (EncryptedContent genericContent) = argument
    genericContent |> PlainContent

let private encryptBlockMock _ (argument: PlainContent) =
    let (PlainContent genericContent) = argument
    genericContent |> EncryptedContent


let private hookCollectionMock
    (fg: MappingStoreDagNodeRefMock -> IStoreShard)
    : HookCollection =

    {
        ParseOuter = parseOuterMock fg
        DecryptBlock = decryptBlockMock
        EncryptBlock = encryptBlockMock
    }


// This shall emulate the GenericStoreShardValue type.
type BranchType =
    | LeafMock of DagNodeRefMock option
    | TreeMock of MappingStoreDagNodeRefMock


let testIteratorCollection = [
    (fun argument mappingKeyAsUInt64 ->
        match argument, mappingKeyAsUInt64 with
        | MappingStoreDagNodeRefMock(asUInt64), todo when todo = asUInt64 ->
            Some (asUInt64 |> DagNodeRefMock) |> LeafMock
        | MappingStoreDagNodeRefMock(asUInt64), todo when todo > asUInt64 ->
            (asUInt64+0x01UL) |> MappingStoreDagNodeRefMock |> TreeMock
        | _ ->
            None |> LeafMock
    )
]

let testVerificationSeqs = [
    (fun argument ->
        match argument with
        | MappingStoreDagNodeRefMock(0x00UL) ->
            Seq.empty
        | MappingStoreDagNodeRefMock(asUInt64) ->
            seq { 0x00UL .. (asUInt64-0x01UL) }
            |> Seq.map MappingStoreDagNodeRefMock
    )
]

let testKeys: PlainContentId seq =
    seq { 0x00UL .. 0x10UL .. 0xf0UL }
    |> Seq.map (fun argument ->
        System.BitConverter.GetBytes(argument)
        |> RawContentId
        |> PlainContentId
    )

let testValues: DagNodeRef seq =
    seq { 0x00UL .. 0x10UL .. 0xf0UL }
    |> Seq.map (fun argument ->
        argument
        |> fromUInt64
        |> makeDagNodeRef
    )


let private parentToUInt (item: ParentCollectionItem) =
    let (MappingStoreDagNodeRef serialisedForm) = item.SerialisedForm
    // Only use the target content id.
    serialisedForm.Target
    |> encryptedContentIdToUInt
    |> MappingStoreDagNodeRefMock


[<SetUp>] // nothing here yet
let Setup () = ()


type private IteratorType = MappingStoreDagNodeRefMock -> uint64 -> BranchType

type private SequenceType =
    MappingStoreDagNodeRefMock -> MappingStoreDagNodeRefMock seq


[<Test>]
let ``ensure the correct ordering of parent collection items``
    ([<ValueSource(nameof testIteratorCollection)>] iterator: IteratorType)
    ([<ValueSource(nameof testVerificationSeqs)>] expected: SequenceType)
    ([<ValueSource(nameof testKeys)>] keyOrIndex: PlainContentId)
    ([<ValueSource(nameof testValues)>] mappingValue: DagNodeRef)
    : unit =

    let storeShardMock
        (parentToUInt: ParentCollectionItem -> MappingStoreDagNodeRefMock)
        (iterator: IteratorType)
        (expected: SequenceType)
        (expectedKeyAsUInt: uint64)
        (expectedValue: DagNodeRef)
        (rootAfterSuccess: DagNodeRef)
        (rootAfterFailure: DagNodeRef)
        (characteristicValue: MappingStoreDagNodeRefMock)
        =

        let objectMockHelper
            (testType: string)
            (parentCollection: ImmutableStack<ParentCollectionItem>)
            key
            value
            =

            // Run some simple checks first.
            Assert.AreEqual(expectedKeyAsUInt, key |> plainContentIdToUInt)
            Assert.AreEqual(expectedValue, value)

            // TODO: document LIFO
            let seq1 =
                parentCollection
                |> List.ofSeq
                |> List.map parentToUInt
                |> List.rev
            let seq2 = characteristicValue |> expected

            TestContext.WriteLine(
                "[action: " +
                testType +
                "] expected parent collection item count: " +
                string (seq2 |> Seq.length)
            )
            TestContext.WriteLine(
                "[action: " +
                testType +
                "] actual parent collection item count: " +
                string (parentCollection.Count())
            )
            Assert.IsTrue(
                seq1.SequenceEqual(seq2),
                "wrong parent collection ordering"
            )

            rootAfterSuccess |> MappingStoreDagNodeRef

        // This anonymous mock object will be injected during testing.
        { new IStoreShard with
            member this.Kind =
                System.Guid("eaca8d82-4a7c-42e2-b20c-43efa0971472")
                |> ShardingKind

            member this.Find(parentCollection, key) =
                let next =
                    key |> plainContentIdToUInt |> iterator characteristicValue
                match next with
                | LeafMock(Some(DagNodeRefMock(asUInt64))) ->
                    Some (asUInt64 |> fromUInt64 |> makeDagNodeRef) |> Leaf
                | LeafMock(None) -> None |> Leaf
                | TreeMock(MappingStoreDagNodeRefMock(asUInt64)) ->
                    asUInt64
                    |> fromUInt64
                    |> makeDagNodeRef
                    |> MappingStoreDagNodeRef
                    |> Tree

            member this.Insert(_, _, _, parentCollection, key, value) = {
                Root = (objectMockHelper "insert" parentCollection key value)
                Pinset = ImmutableQueue.Create<PinsetItem>()
            }

            member this.Update(_, _, _, parentCollection, key, value) = {
                Root = (objectMockHelper "update" parentCollection key value)
                Pinset = ImmutableQueue.Create<PinsetItem>()
            }

            member this.Delete(_, _, _, parentCollection, key, value) = {
                Root = (objectMockHelper "delete" parentCollection key value)
                Pinset = ImmutableQueue.Create<PinsetItem>()
            }
        }

    let testAction actionToTest: unit =
        // Prepare a state container for the result.
        let rootForTesting =
            ref (0x00000000UL |> fromUInt64 |> makeDagNodeRef |> MappingStoreDagNodeRef)

        let (ignoreMe: bool) =
            let storeShardForTesting =
                // Instantiate our mock IStoreShard implementation using
                // the appropriate parameters.
                storeShardMock
                    parentToUInt
                    iterator
                    expected
                    (keyOrIndex |> plainContentIdToUInt)
                    mappingValue
                    (0xffffffffUL |> fromUInt64 |> makeDagNodeRef)
                    (0xdeadbeefUL |> fromUInt64 |> makeDagNodeRef)

            let hookCollectionForTesting =
                // Inject our mock IStoreShard implementation.
                hookCollectionMock
                    storeShardForTesting

            actionToTest
                hookCollectionForTesting
                { new IStorageHooks with
                    member this.GetBlock(argument) = getBlockMock argument

                    member this.PutBlock(argument) = putBlockMock argument
                }
                { new IPinningHooks with
                    member this.UnpinContent(_) = ()

                    member this.PinContent(_) = ()
                }
                rootForTesting
                keyOrIndex
                mappingValue

        // This will probably become obsolete in the future.
        ignoreMe |> ignore

        let (MappingStoreDagNodeRef result) = Volatile.Read(rootForTesting)

        Assert.AreEqual(
            (0xffffffffUL |> fromUInt64 |> makeDagNodeRef),
            result
        )

    //let _ = Actions.insert |> testAction
    let _ = Actions.update |> testAction
    let _ = Actions.delete |> testAction

    ()
