namespace VengefulFi.Ipld.DataModel


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


open VengefulFi.Ipld


// the basic ipld data kinds
type Kind =
    | Null = 0x00
    | Bool = 0x01
    | Integer = 0x02
    | Float = 0x03
    | String = 0x04
    | Bytes = 0x05
    | List = 0x06
    | Map = 0x07
    | Link = 0x08


// the common interface for all kinds of nodes
type INode =
    abstract member AsNull: unit
    abstract member IsNull: bool

    abstract member Kind: Kind

    abstract member IsAbsent: bool

    // access the node's underlying data

    abstract member AsBool: bool

    abstract member AsSInt08: sbyte
    abstract member AsSInt16: int16
    abstract member AsSInt32: int32
    abstract member AsSInt64: int64

    abstract member AsUInt08: byte
    abstract member AsUInt16: uint16
    abstract member AsUInt32: uint32
    abstract member AsUInt64: uint64

    abstract member AsBigInt: bigint

    abstract member AsSingle: single
    abstract member AsDouble: double

    abstract member AsString: string

    abstract member AsRawContent: RawContent

    abstract member AsGenericContentId: GenericContentIdFuture
    abstract member AsRawContentId: RawContentId

    abstract member AsList: Generic.IReadOnlyList<INode>

    abstract member AsDictionary: Generic.IReadOnlyDictionary<string, INode>


type INodeBuilder =
    // instantiate a new tree of immutable nodes using the current builder state
    abstract member ToNode: unit -> INode

    // scalar value assignment

    abstract member Assign: bool -> unit

    abstract member Assign: sbyte -> unit
    abstract member Assign: int16 -> unit
    abstract member Assign: int32 -> unit
    abstract member Assign: int64 -> unit

    abstract member Assign: byte -> unit
    abstract member Assign: uint16 -> unit
    abstract member Assign: uint32 -> unit
    abstract member Assign: uint64 -> unit

    abstract member Assign: bigint -> unit

    abstract member Assign: single -> unit
    abstract member Assign: double -> unit

    abstract member Assign: string -> unit

    // binary data must be provided as a .NET stream
    abstract member Assign: RawContent -> unit

    // content id assignment
    abstract member Assign: GenericContentIdFuture -> unit
    abstract member Assign: RawContentId -> unit

    // the current capacity of this collection
    abstract member Capacity: unit -> uint

    // the current number of elements in this collection
    abstract member Length: unit -> uint

    // set the minimum capacity of this collection
    abstract member EnsureCapacity: uint -> uint

    abstract member Append: INode -> unit
    abstract member Extend: INode -> unit

    abstract member Insert: uint * INode -> unit
    abstract member Insert: string * INode -> unit

    abstract member Update: uint * INode -> unit
    abstract member Update: string * INode -> unit

    abstract member Remove: uint -> unit
    abstract member Remove: string -> unit


exception NotInitialisedException


type Base() =
    interface INode with
        member this.AsNull =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.IsNull =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.Kind =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.IsAbsent =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsBool =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsUInt08 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsUInt16 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsUInt32 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsUInt64 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsSInt08 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsSInt16 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsSInt32 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsSInt64 =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsBigInt =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsSingle =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsDouble =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsString =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsGenericContentId =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsRawContent =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsRawContentId =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsList =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )

        member this.AsDictionary =
            raise (
                System.NotSupportedException
                    "operation not supported by this implementation"
            )
