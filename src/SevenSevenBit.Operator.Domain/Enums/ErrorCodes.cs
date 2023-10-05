namespace SevenSevenBit.Operator.Domain.Enums;

public enum ErrorCodes
{
    // MODEL STATE VALIDATIONS 1****
    ModelStateInvalid = 10000,
    InvalidGuid = 10001,
    FilterIsRequired = 10002,

    // MODEL STATE - USERNAME VALIDATIONS 101**
    UsernameRequired = 10101,
    UsernameLengthInvalid = 10102,
    UsernameAlreadyRegistered = 10103,
    UsersRequired = 10104,
    UserIdRequired = 10105,
    UserIdNotFound = 10106,

    // MODEL STATE - ADDRESS VALIDATIONS 102**
    AddressRequired = 10201,
    AddressInvalid = 10202,
    AddressAlreadyRegistered = 10203,

    // MODEL STATE - STARK SIG VALIDATIONS 103**
    StarkSignatureRequired = 10301,
    StarkSignatureRRequired = 10302,
    StarkSignatureSRequired = 10303,
    StarkSignatureInvalid = 10304,

    // MODEL STATE - STARK KEY VALIDATIONS 104**
    StarkKeyRequired = 10401,
    StarkKeyLengthInvalid = 10402,
    StarkKeyFormatInvalid = 10403,
    StarkKeyAlreadyInUse = 10404,

    // MODEL STATE - EIP712 VALIDATIONS 105**
    Eip712SignatureRequired = 10501,
    Eip712SignatureInvalid = 10502,

    // MODEL STATE - ASSET VALIDATIONS 106**
    AssetAlreadyWhitelisted = 10601,
    AssetDisabled = 10602,
    InvalidAssetType = 10603,
    AssetAddressIsNotAContract = 10604,
    AssetContractIsNotMintable = 10605,
    AssetIdRequired = 10606,
    AssetNameRequired = 10607,
    AssetSymbolRequired = 10608,
    AssetQuantumRequired = 10609,
    AssetUriRequired = 10610,
    AssetAlreadyRegistered = 10611,
    AssetNotRegistered = 10612,
    AssetNotConfirmed = 10613,
    AssetNotSupported = 10614,
    AssetNotFound = 10615,

    // MODEL STATE - QUANTUM VALIDATIONS 107**
    AssetQuantumInvalid = 10701,

    // MODEL STATE - AMOUNT VALIDATIONS 108**
    OperationAmountRequired = 10801,
    OperationAmountInvalid = 10802,
    OperationAmountUnquantizable = 10803,

    // MODEL STATE - VAULT VALIDATIONS 109**
    VaultNotRegistered = 10901,
    VaultOutOfBounds = 10902,
    VaultIdRequired = 10903,

    // MODEL STATE - TOKEN ID VALIDATIONS 110**
    TokenIdRequired = 11001,

    // MODEL STATE - STARKTYPE VALIDATIONS 120**
    StarkTypeInvalid = 12001,
    StarkTypeInvalidFormat = 12002,
    StarkTypeRequired = 12003,

    // MODEL STATE - MINT VALIDATIONS 121**
    MintsRequired = 12101,
    MintingBlobRequired = 12102,
    DuplicatedUserIds = 12103,
    DuplicatedMints = 12104,
    ExistingMintingBlob = 12105,

    // MODEL STATE - DATA AVAILABILITY VALIDATIONS 122**
    DataAvailabilityRequired = 12201,

    // MODEL STATE - FEE VALIDATIONS 123**
    FeeActionRequired = 12301,
    FeeBasisPointsRequired = 12302,
    FeeNotConfigured = 12303,
    FeeBasisPointOutOfRange = 12304,

    // MODEL STATE - ORDER VALIDATIONS 124**
    FeeVaultIdRequired = 12401,
    FeeQuantizedAmountRequired = 12402,
    BuyVaultIdRequired = 12403,
    BuyAmountRequired = 12404,
    SellVaultIdRequired = 12405,
    SellAmountRequired = 12406,
    OrderARequired = 12407,
    OrderBRequired = 12408,
    OrderAFeeAmountRequired = 12409,
    OrderBFeeAmountRequired = 12410,
    OrderAFeeDestinationVaultIdRequired = 12411,
    OrderBFeeDestinationVaultIdRequired = 12412,

    // MODEL STATE - TRANSACTION VALIDATIONS 130**
    SenderVaultIdRequired = 13001,
    ReceiverVaultIdRequired = 13002,
    ExpirationTimestampRequired = 13003,
    NonceRequired = 13004,
    TransactionIdNotFound = 13005,

    // MODEL STATE - TRANSACTION DETAILS VALIDATIONS 140**
    AssetTypeRequired = 14001,
    SenderDataAvailabilityModeRequired = 14002,
    ReceiverDataAvailabilityModeRequired = 14003,
    MintingBlobNotFound = 14004,

    // MODEL STATE - HEX STRING VALIDATIONS 150**
    InvalidHexString = 15001,

    // MODEL STATE - TENANT VALIDATIONS 160**
    TenantNameAlreadyRegistered = 16001,
    TenantIdNotFound = 16002,
    TenantOwnerRequired = 16003,

    // MODEL STATE - StarkEx VALIDATIONS 170**
    StarkExInstanceNotFound = 17001,
    StarkExContractAlreadyRegistered = 17002,
    StarkExNameAlreadyRegistered = 17003,
    StarkExAddressRequired = 17004,
    StarkExContractTokensAdminKeyRequired = 17005,
    StarkExInstanceNameRequired = 17006,
    ValidiumTreeHeightRequired = 17007,
    ZkRollupTreeHeightRequired = 17008,
    ValidiumTreeHeightOutOfRange = 17009,
    ZkRollupTreeHeightOutOfRange = 17010,
    StarkExHostnameRequired = 17011,
    StarkExVersionRequired = 17012,
    StarkExHostnameInvalid = 17013,
    StarkExVersionNotSupported = 17014,
    IsUniqueMintingEnabledRequired = 17015,
    StarkExCertificateRequired = 17016,
    StarkExCertificateIsInvalid = 17017,
    StarkExInstanceTokenAdminPrivateKeySecretNotFound = 17018,
    StarkExDeploymentBlockMissing = 17019,
    StarkExDeploymentBlockOutOfRange = 17020,

    // MODEL STATE - ORDERBOOK VALIDATIONS 180**
    OrderbookIdRequired = 18001,
    BaseAssetIdRequired = 18002,
    QuoteAssetIdRequired = 18003,
    BaseAssetPrecisionRequired = 18004,
    QuoteAssetPrecisionRequired = 18005,

    // MODEL STATE - ORDER VALIDATIONS 190**
    OrderIdRequired = 19001,
    OrderSideRequired = 19002,
    OrderPriceRequired = 19003,
    OrderAmountRequired = 19004,

    // BUSINESS LOGIC STATE VALIDATIONS 2****
    // BALANCE STATE VALIDATIONS 201**
    InsufficientBalance = 20101,

    // TRANSFER STATE VALIDATIONS 203**
    TimestampOutsideValidRange = 20301,
    TransferIntoSameVault = 20302,
    ConflictingVaultAssets = 20303,
    ConflictingVaultOwners = 20304,

    // FEE STATE VALIDATIONS 204**
    FeeAlreadyConfigured = 20401,

    // WITHDRAW VALIDATIONS 205**
    MintingBlobInvalid = 20501,
    TokenIdInvalid = 20502,

    // TENANT OWNER VALIDATIONS 206**
    TenantOwnerUnauthorized = 20601,

    // MARKETPLACE VALIDATIONS 207**
    SameBaseAndQuoteAssets = 20701,

    // MARKETPLACE VALIDATIONS 208**
    CurrencyIdRequired = 20801,
    SellerIdRequired = 20802,
    QuantityRequired = 20803,
    PriceRequired = 20804,
    ProductAmountRequired = 20805,
    CurrencyAmountRequired = 20806,
    ProductVaultIdRequired = 20807,
    CurrencyVaultIdRequired = 20808,
    ProductDataRequired = 20809,
    ProductNameTooLong = 20810,
    ProductDescriptionTooLong = 20811,
    SellOfferDoesntExist = 20812,
    SellOfferIsNotActive = 20813,
    WrongVaultAsset = 20814,
    BuyerIdRequired = 20815,
    OfferIdRequired = 20816,
    MarketplaceNotFound = 20817,
    TradableAssetIdRequired = 20818,

    // INTERNAL ERRORS 5****
    InternalError = 50000,
}