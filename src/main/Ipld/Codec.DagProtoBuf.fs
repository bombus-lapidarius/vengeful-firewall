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


open ProtoBuf


open VengefulFi.Ipld


// TODO: optional (see dag-pb spec)
// TODO: repeated -> List<PBLink> OK?


[<ProtoContract>]
[<ProtoInclude(63, "PBNodeRepr")>]
type private PBLinkRepr(hash, name, targetSize) =
    [<ProtoMember(1)>]
    member val Hash: byte array = hash with get, set

    [<ProtoMember(2)>]
    member val Name: string = name with get, set

    [<ProtoMember(3)>]
    member val TargetSize: uint64 = targetSize with get, set


[<ProtoContract>]
type private PBNodeRepr(data, linkList) =
    [<ProtoMember(1)>]
    member val Data: byte array = data with get, set

    [<ProtoMember(2)>]
    member val LinkList: List<PBLinkRepr> = linkList with get, set


type PBLinkHash() =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Link

        member this.IsNull = false

type PBLinkName() =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.String

        member this.IsNull = false

type PBLinkTargetSize() =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Integer

        member this.IsNull = false


type PBLink() =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Map

        member this.IsNull = false


type PBLinkList() =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.List

        member this.IsNull = false


type PBNode() =
    inherit DataModel.Base()

    interface DataModel.INode with
        member this.Kind = DataModel.Kind.Map

        member this.IsNull = false
