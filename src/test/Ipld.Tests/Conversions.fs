module VengefulFi.Ipld.Tests.Conversions


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


open NUnit.Framework


open VengefulFi.Ipld


type CidMapping =
    { Generic: GenericContentIdFuture
      Raw: RawContentId }


let private makeHash hn hs (dg: string) : Hash =
    { Name = hn
      Size = hs
      Digest = System.Convert.FromHexString(dg) |> Digest }


let private makeRawCid (bs: string) : RawContentId =
    System.Convert.FromHexString(bs) |> RawContentId


let private makeCidMapping
    (cv: CidVersion)
    (mc: Multicodec)
    (hn: HashName)
    (hs: uint32)
    (dg: string)
    (rc: string)
    : CidMapping =
    let hash = makeHash hn hs dg

    { Generic =
        { CidVersion = cv
          Multicodec = mc
          Hash = hash }
      Raw = makeRawCid rc }


let cids: list<CidMapping> =
    [ makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "0DCC905C79DCC449A189D6E4733AFBBC81E1AD78EA800FA5BF6F20D51738533E"
          "017012200dcc905c79dcc449a189d6e4733afbbc81e1ad78ea800fa5bf6f20d51738533e"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "11F4EBE2067798E21F76059CED01D7DB8BDB4AB8B422A0653E631DD485EADA37"
          "0170122011f4ebe2067798e21f76059ced01d7db8bdb4ab8b422a0653e631dd485eada37"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "207C4933087FD02F35AD2B6751346B7744126C04BE9BC1BD9F020A388D7AB083"
          "01701220207c4933087fd02f35ad2b6751346b7744126c04be9bc1bd9f020a388d7ab083"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "77BEBB9DF538F9DB9CE4C5F4E64EE2591CE7A9875A8C637E52955D4EB6646CFF"
          "0170122077bebb9df538f9db9ce4c5f4e64ee2591ce7a9875a8c637e52955d4eb6646cff"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "9C069666FDA1E33436BE02003EC5AE81C51429D6B68B312ECD0AA7B681BDB993"
          "017012209c069666fda1e33436be02003ec5ae81c51429d6b68b312ecd0aa7b681bdb993"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "9C1A6EE09BFEEFF8B0B9C45BDE131F12B36E6396F38E98AF26DC6525972EC1F1"
          "017012209c1a6ee09bfeeff8b0b9c45bde131f12b36e6396f38e98af26dc6525972ec1f1"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "D2F321C51E6ACDC9465DA426D2DC9A57BF82EDE2FF22A378B70992FE895259CA"
          "01701220d2f321c51e6acdc9465da426d2dc9a57bf82ede2ff22a378b70992fe895259ca"
      makeCidMapping
          CidVersion.Cid1
          Multicodec.DagPb
          HashName.Sha256
          32ul
          "D55EEB50DF176B41BF5D8F0B93C18FFBEB53BC704440A206702BCFEB88FA9370"
          "01701220d55eeb50df176b41bf5d8f0b93c18ffbeb53bc704440a206702bcfeb88fa9370" ]


[<SetUp>] // nothing here yet
let Setup () = ()


[<Test>]
let genericToRaw
    ([<ValueSource(nameof (cids))>] cidMapping: CidMapping)
    : unit =

    let (RawContentId ex) = cidMapping.Raw
    let (RawContentId ac) = cidMapping.Generic |> Conversions.composeBinaryCid

    Assert.AreEqual(ex, ac)


[<Test>]
let rawToGeneric
    ([<ValueSource(nameof (cids))>] cidMapping: CidMapping)
    : unit =

    let ex = cidMapping.Generic
    let ac = cidMapping.Raw |> Conversions.parseBinaryCid

    let (Digest exDigest) = ex.Hash.Digest
    let (Digest acDigest) = ac.Hash.Digest

    Assert.AreEqual(exDigest, acDigest)

    Assert.AreEqual(ex.CidVersion, ac.CidVersion)
    Assert.AreEqual(ex.Multicodec, ac.Multicodec)

    Assert.AreEqual(ex.Hash.Name, ac.Hash.Name)
    Assert.AreEqual(ex.Hash.Size, ac.Hash.Size)
