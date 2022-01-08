module Encryption.Block


open System.Security.Cryptography


type PlainContent = Types.PlainContent
type EncryptedContent = Types.EncryptedContent


type PlainContentId = Types.PlainContentId
type EncryptedContentId = Types.EncryptedContentId


type Cipher = Types.Cipher


type CalcPlainCidType = PlainContent -> PlainContentId


// parameterise based on these function types to enable efficient unit testing
type FetchCidType = Cipher -> byte[] -> PlainContentId -> EncryptedContentId
type StoreCidType = Cipher -> byte[] -> PlainContentId -> EncryptedContentId -> unit


// parameterise based on these function types to enable efficient unit testing
type GetRawType = EncryptedContentId -> EncryptedContent
type PutRawType = EncryptedContent -> EncryptedContentId


let private createCryptoEngine (cipher: Cipher) (key: byte[]): Aes = // TODO: generalise
    match cipher with
    | Types.Cipher.AesCbc ->
        let aes = Aes.Create()
        // TODO: verify that the key size is valid for the chosen cipher
        aes.Key <- key
        // TODO: set block size?
        aes.IV <- key // TODO: essiv for IV generation
        aes.Mode <- CipherMode.CBC
        // TODO: set feedback size?
        // TODO: set padding?
        aes // cbc


let decryptBlock (cipher: Cipher) (key: byte[]) (input: EncryptedContent):
    PlainContent =

    use aes = createCryptoEngine cipher key // TODO: dangling ref?
    let cbcDecryptor = aes.CreateDecryptor()
    let (Types.EncryptedContent inputStream) = input
    let output = new CryptoStream(inputStream, cbcDecryptor, CryptoStreamMode.Read, false) // TODO: does this return a usable stream object?
    Types.PlainContent output // construct the tagged union result


let encryptBlock (cipher: Cipher) (key: byte[]) (input: PlainContent):
    EncryptedContent =

    use aes = createCryptoEngine cipher key // TODO: dangling ref?
    let cbcEncryptor = aes.CreateEncryptor()
    let (Types.PlainContent inputStream) = input
    let output = new CryptoStream(inputStream, cbcEncryptor, CryptoStreamMode.Read, false) // TODO: does this return a usable stream object?
    Types.EncryptedContent output // construct the tagged union result


let decryptGet (getRaw: GetRawType) (fetchCid: FetchCidType)
    (cache: Cache.PlainDataCache) (cipher: Cipher) (key: byte[])
    (plainCid: PlainContentId): PlainContent =

    match Cache.getFromCache cache plainCid with // PlainContent
    | None -> // we need to fetch the data from IPFS
        fetchCid cipher key plainCid // returns EncryptedContentId
        |> getRaw // returns EncryptedContent
        // TODO: hash here to verify that the encrypted content returned matches the fetched cid?
        |> (decryptBlock cipher key) // returns PlainContent
    | Some(data) -> data


let encryptPut (putRaw: PutRawType) (storeCid: StoreCidType)
    (calcPlainCid: CalcPlainCidType) // TODO: asymmetry OK here?
    (cache: Cache.PlainDataCache) (cipher: Cipher) (key: byte[])
    (data: PlainContent): PlainContentId =

    let plainCid = calcPlainCid data // we can't pass on plaintext to the IPFS
    Cache.putIntoCache cache plainCid data // returns unit
    encryptBlock cipher key data // returns EncryptedContent
    |> putRaw // returns EncryptedContentId
    |> (storeCid cipher key) plainCid // returns unit
    plainCid // TODO: hash here to verify the cid returned by the underlying IPFS node?
