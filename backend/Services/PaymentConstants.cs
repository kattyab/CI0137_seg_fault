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

    public const string DeclinedCardPrefix = "400000";

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
}
