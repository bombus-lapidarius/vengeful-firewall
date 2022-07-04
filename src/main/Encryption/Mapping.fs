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


open System.Collections


open VengefulFi.Encryption


// This needs to be a reference type, so that references to its instances can be
// modified atomically using a compare-and-swap operation.
[<NoComparison>]
type DagNodeRef = {
    Cipher: byte [] // TODO: key, cipher and some "seed value"
    Target: EncryptedContentId
}


// TODO: use an off-the-shelf F# reference cell?
// TODO: include a reference to the previous root?
[<NoComparison>]
type Root = {
    mutable TopLevelNode: DagNodeRef
}


// TODO: extend at run time (using attributes)
type ShardingKind =
    | HAMT = 0x00uL


// shards can either represent leaves (actual data or a dead end) or trees
// (tree in a generalised sense, actually a dag)
type GenericStoreShardValue =
    | Leaf of DagNodeRef option
    | Tree of DagNodeRef


// for convenience

type DecryptionType = byte [] -> EncryptedContent -> PlainContent
type EncryptionType = byte [] -> PlainContent -> EncryptedContent


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

    // TODO
    abstract member DeserializeShard: PlainContent -> IStoreShard
    abstract member SerializeShard: IStoreShard -> PlainContent
