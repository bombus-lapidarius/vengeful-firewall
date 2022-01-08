module Encryption.Cache


type PlainContent = Types.PlainContent
type EncryptedContent = Types.EncryptedContent


type PlainContentId = Types.PlainContentId
type EncryptedContentId = Types.EncryptedContentId


type PlainDataCache = int option // TODO


let getFromCache (cache: PlainDataCache) (plainCid: PlainContentId): PlainContent option =
    match cache with
    | None -> None
    | Some(myCache) -> None // TODO: create an actual implementation

let putIntoCache (cache: PlainDataCache) (plainCid: PlainContentId) (plainData: PlainContent) =
    match cache with
    | None -> ()
    | Some(myCache) -> () // TODO: store and return unit
