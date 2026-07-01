namespace Backend.Api.Services;

public static class PaymentConstants
{
    public const int MinCardNumberLength = 13;
    public const int MaxCardNumberLength = 19;

    public const int CardBinLength = 6;
    public const int CardLastDigitsLength = 4;

    public const int MinCvcLength = 3;
    public const int MaxCvcLength = 4;

    public const int MinMonth = 1;
    public const int MaxMonth = 12;

    public const int TwoDigitYearLimit = 100;
    public const int FullYearOffset = 2000;

    public const int LuhnMultiplier = 2;
    public const int LuhnMaxSingleDigit = 9;
    public const int LuhnModuloBase = 10;

    public const decimal MinimumTransactionAmount = 0m;
    public const decimal MaximumTransactionAmount = 500000m;

    public const int AuthorizationCodeLength = 8;

    public const string DeclinedCardSuffix = "0000";

    public const string VisaPrefix = "4";
    public const string MastercardPrefix = "5";
    public const string AmericanExpressPrefix34 = "34";
    public const string AmericanExpressPrefix37 = "37";

    public const string VisaBrand = "Visa";
    public const string MastercardBrand = "Mastercard";
    public const string AmericanExpressBrand = "American Express";
    public const string UnknownBrand = "Unknown";

    public const string NonDigitRegexPattern = @"\D";

    public static string CvcRegexPattern =>
        $@"^\d{{{MinCvcLength},{MaxCvcLength}}}$";

    public static readonly IReadOnlyDictionary<string, PaymentBinInfo> CostaRicaBins =
        new Dictionary<string, PaymentBinInfo>
        {
            { "400943", new PaymentBinInfo("AVAL CARD (COSTA RICA), S.A.", "Visa") },
            { "401630", new PaymentBinInfo("ATH COSTA RICA, S.A.", "Visa") },
            { "401631", new PaymentBinInfo("ATH COSTA RICA, S.A.", "Visa") },
            { "401632", new PaymentBinInfo("ATH COSTA RICA, S.A.", "Visa") },
            { "401681", new PaymentBinInfo("BANCO DEL ISTMO (COSTA RICA), S.A.", "Visa") },
            { "402667", new PaymentBinInfo("BANCO NACIONAL DE COSTA RICA", "Visa") },
            { "403816", new PaymentBinInfo("BANCO CUSCATLAN DE COSTA RICA, S.A.", "Visa") },
            { "403817", new PaymentBinInfo("BANCO CUSCATLAN DE COSTA RICA, S.A.", "Visa") },
            { "403818", new PaymentBinInfo("BANCO CITIBANK DE COSTA RICA, S.A.", "Visa") },
            { "405705", new PaymentBinInfo("BANCO POPULAR Y DE DESARROLLO COMUNAL", "Visa") },
            { "407287", new PaymentBinInfo("BANCA PROMERICA, S.A.", "Visa") },
            { "407550", new PaymentBinInfo("BANCA PROMERICA, S.A.", "Visa") },
            { "409627", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "409731", new PaymentBinInfo("BANCO POPULAR Y DE DESARROLLO COMUNAL", "Visa") },
            { "410372", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "410864", new PaymentBinInfo("BANCO NACIONAL DE COSTA RICA", "Visa") },
            { "410865", new PaymentBinInfo("BANCO NACIONAL DE COSTA RICA", "Visa") },
            { "411061", new PaymentBinInfo("BANCO NACIONAL DE COSTA RICA", "Visa") },
            { "411604", new PaymentBinInfo("AVAL CARD (COSTA RICA), S.A.", "Visa") },
            { "411608", new PaymentBinInfo("SCOTIABANK DE COSTA RICA, S.A.", "Visa") },
            { "415276", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "415277", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "415278", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "424101", new PaymentBinInfo("SCOTIABANK DE COSTA RICA, S.A.", "Visa") },
            { "424102", new PaymentBinInfo("SCOTIABANK DE COSTA RICA, S.A.", "Visa") },
            { "432394", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "438098", new PaymentBinInfo("BANCO DE COSTA RICA", "Visa") },
            { "441576", new PaymentBinInfo("BANCO POPULAR Y DE DESARROLLO COMUNAL", "Visa") },
            { "441577", new PaymentBinInfo("BANCO POPULAR Y DE DESARROLLO COMUNAL", "Visa") },
            { "441578", new PaymentBinInfo("BANCO POPULAR Y DE DESARROLLO COMUNAL", "Visa") }
        };
}
