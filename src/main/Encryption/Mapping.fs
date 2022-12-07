namespace VengefulFi.Encryption.Mapping


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


open VengefulFi.Encryption


// This data structure only contains two references and should not take up
// more than 16 bytes on any cpu architecture that uses at most 64 bits in
// its memory addresses.
[<NoComparison>]
[<Struct>]
type DagNodeRef = {
    Cipher: byte [] // TODO: key, cipher and some "seed value"
    Target: EncryptedContentId
}


// This needs to be a reference type, so that references to its instances can be
// modified atomically using a compare-and-swap operation.
[<NoComparison>]
type MappingStoreDagNodeRef = | MappingStoreDagNodeRef of DagNodeRef


// TODO: use an off-the-shelf F# reference cell?
// TODO: include a reference to the previous root?
[<NoComparison>]
type Root = {
    mutable TopLevelNode: MappingStoreDagNodeRef
}


// TODO: extend at run time (using attributes)
type ShardingKind =
    | HAMT = 0x00uL


// "outer" parser: determine the sharding algorithm in order to call the correct
// "inner" parser at runtime
// "inner" parser: call the appropriate constructor or parser method


// This type's instances need to be immutable to guarantee consistency
// when its members are called inside other functions.
[<NoComparison>]
[<NoEquality>]
type HookCollectionTemplate<'TParserResult> = {
    ParseOuter: PlainContent -> ShardingKind * PlainContent
    ParseInner: ShardingKind -> PlainContent -> 'TParserResult
    GetBlock: EncryptedContentId -> EncryptedContent
    PutBlock: EncryptedContent -> EncryptedContentId
    DecryptBlock: byte [] -> EncryptedContent -> PlainContent
    EncryptBlock: byte [] -> PlainContent -> EncryptedContent
}


// This shall serve as a vehicle for potential future forwards-compatibility
// in case some implementations choose to rely on the caching of function call
// results in their parser functions to improve runtime performance.
// Providing both the deserialised as well as the original serialised form
// of mapping store dag nodes ensures that both forms are always available
// without indirections wherever a parent collection is passed around.
[<Struct>]
type ParentCollectionItemTemplate<'TParsedForm> = {
    DeserialisedForm: 'TParsedForm
    SerialisedForm: MappingStoreDagNodeRef
}


// shards can either represent leaves (actual data or a dead end) or trees
// (tree in a generalised sense, actually a dag)
type GenericStoreShardValue =
    | Leaf of DagNodeRef option
    | Tree of MappingStoreDagNodeRef


type PinsetAction =
    | Pin = 0x00uy
    | Unpin = 0x01uy

[<Struct>]
type PinsetItem = {
    Target: EncryptedContentId
    Action: PinsetAction
}

[<Struct>]
type ModificationResult = {
    Root: MappingStoreDagNodeRef
    Pinset: ImmutableQueue<PinsetItem>
}


// this is where the actual implementations live (i.e. whether this is
// a search tree, a HAMT etc.)
type IStoreShard =
    abstract member Kind: ShardingKind

    // arg: parentCollection
    // arg: index (or key)
    abstract member Find:
        ImmutableStack<ParentCollectionItemTemplate<IStoreShard>> ->
        PlainContentId ->
            GenericStoreShardValue

    // arg: hookCollection
    // arg: parentCollection
    // arg: index (or key)
    // arg: new or updated value
    abstract member Insert:
        HookCollectionTemplate<IStoreShard> ->
        ImmutableStack<ParentCollectionItemTemplate<IStoreShard>> ->
        PlainContentId ->
        DagNodeRef ->
            ModificationResult

    // arg: hookCollection
    // arg: parentCollection
    // arg: index (or key)
    // arg: new or updated value
    abstract member Update:
        HookCollectionTemplate<IStoreShard> ->
        ImmutableStack<ParentCollectionItemTemplate<IStoreShard>> ->
        PlainContentId ->
        DagNodeRef ->
            ModificationResult

    // arg: hookCollection
    // arg: parentCollection
    // arg: index (or key)
    // arg: new or updated value
    abstract member Delete:
        HookCollectionTemplate<IStoreShard> ->
        ImmutableStack<ParentCollectionItemTemplate<IStoreShard>> ->
        PlainContentId ->
        DagNodeRef ->
            ModificationResult


type HookCollection = HookCollectionTemplate<IStoreShard>

type ParentCollectionItem = ParentCollectionItemTemplate<IStoreShard>
