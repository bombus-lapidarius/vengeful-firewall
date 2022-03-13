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
    abstract member Kind: Kind

    // access the node's underlying data
    abstract member AsNull: unit
    abstract member AsBool: bool
    abstract member AsInteger: bigint // TODO
    abstract member AsFloat: double // TODO

    abstract member AsString: string
    abstract member AsRawContent: RawContent

    // access the node's underlying data
    abstract member AsCSharpList: System.Collections.Generic.List<INode>
    abstract member AsFSharpList: INode list
    // TODO: "as dict"

    abstract member Length: uint64

    // access the node's underlying data
    abstract member AsGenericContentId: GenericContentIdFuture
    abstract member AsRawContentId: RawContentId

    abstract member IsNull: bool

    abstract member LookupByString: string -> INode option
    //abstract member LookupByNode
    //abstract member LookupBySegment
    abstract member LookupByIndex: uint64 -> INode option

//abstract member IsAbsent: bool


exception UnsupportedOperationException of Kind


type Base() =
    interface INode with
        member this.Kind =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsNull =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsBool =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsInteger =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsFloat =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsString =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsRawContent =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsCSharpList =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsFSharpList =
            raise (UnsupportedOperationException Kind.Null)

        member this.Length =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsGenericContentId =
            raise (UnsupportedOperationException Kind.Null)

        member this.AsRawContentId =
            raise (UnsupportedOperationException Kind.Null)

        member this.IsNull =
            raise (UnsupportedOperationException Kind.Null)

        member this.LookupByString s =
            raise (UnsupportedOperationException Kind.Null)

        member this.LookupByIndex id =
            raise (UnsupportedOperationException Kind.Null)
