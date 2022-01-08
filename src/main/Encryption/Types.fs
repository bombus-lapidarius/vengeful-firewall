module Encryption.Types


open System.IO


type PlainContent = PlainContent of Stream
type EncryptedContent = EncryptedContent of Stream


type PlainContentId = PlainContentId of byte[]
type EncryptedContentId = EncryptedContentId of byte[]


type Cipher =
    | AesCbc
    //| AesXts
