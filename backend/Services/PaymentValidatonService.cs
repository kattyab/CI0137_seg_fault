using System.Text.RegularExpressions;
using Backend.Api.DTOs.Payments;

namespace Backend.Api.Services;

public class PaymentValidationService
{
    public PaymentValidationResult Validate(PaymentRequestDto payment)
    {
        string digits = Regex.Replace(
            payment.CardNumber,
            PaymentConstants.NonDigitRegexPattern,
            string.Empty
        );

        if (string.IsNullOrWhiteSpace(payment.CardholderName))
            return PaymentValidationResult.Fail("El nombre del titular es obligatorio.");

        if (digits.Length < PaymentConstants.MinCardNumberLength ||
            digits.Length > PaymentConstants.MaxCardNumberLength)
            return PaymentValidationResult.Fail("El número de tarjeta no tiene una longitud válida.");

        if (!PassesLuhn(digits))
            return PaymentValidationResult.Fail("El número de tarjeta no es válido.");

        if (!IsExpiryValid(payment.ExpMonth, payment.ExpYear))
            return PaymentValidationResult.Fail("La tarjeta está vencida o la fecha no es válida.");

        if (!Regex.IsMatch(payment.Cvc, PaymentConstants.CvcRegexPattern))
            return PaymentValidationResult.Fail("El CVC debe tener 3 o 4 dígitos.");

        string bin = digits[..PaymentConstants.CardBinLength];

        if (!PaymentConstants.CostaRicaBins.TryGetValue(bin, out var binInfo))
        {
            return PaymentValidationResult.Fail(
                "El BIN de la tarjeta no pertenece a una entidad registrada para Costa Rica."
            );
        }

        return PaymentValidationResult.Ok(
            bin: bin,
            last4: digits[^PaymentConstants.CardLastDigitsLength..],
            brand: binInfo.Brand,
            issuerBank: binInfo.BankName
        );
    }

    private bool PassesLuhn(string digits)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = digits.Length - 1; i >= 0; i--)
        {
            int n = digits[i] - '0';

            if (alternate)
            {
                n *= PaymentConstants.LuhnMultiplier;

                if (n > PaymentConstants.LuhnMaxSingleDigit)
                    n -= PaymentConstants.LuhnMaxSingleDigit;
            }

            sum += n;
            alternate = !alternate;
        }

        return sum % PaymentConstants.LuhnModuloBase == 0;
    }

    private bool IsExpiryValid(int month, int year)
    {
        if (month < PaymentConstants.MinMonth || month > PaymentConstants.MaxMonth)
            return false;

        if (year < PaymentConstants.TwoDigitYearLimit)
            year += PaymentConstants.FullYearOffset;

        var lastDay = new DateTime(
            year,
            month,
            DateTime.DaysInMonth(year, month)
        );

        return lastDay >= DateTime.UtcNow.Date;
    }

    private string GetBrand(string digits)
    {
        if (digits.StartsWith(PaymentConstants.VisaPrefix))
            return PaymentConstants.VisaBrand;

        if (digits.StartsWith(PaymentConstants.MastercardPrefix))
            return PaymentConstants.MastercardBrand;

        if (digits.StartsWith(PaymentConstants.AmericanExpressPrefix34) ||
            digits.StartsWith(PaymentConstants.AmericanExpressPrefix37))
            return PaymentConstants.AmericanExpressBrand;

        return PaymentConstants.UnknownBrand;
    }
}

public class PaymentValidationResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string Bin { get; set; } = string.Empty;
    public string Last4 { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string IssuerBank { get; set; } = string.Empty;

    public static PaymentValidationResult Ok(
        string bin,
        string last4,
        string brand,
        string issuerBank)
    {
        return new PaymentValidationResult
        {
            Success = true,
            Bin = bin,
            Last4 = last4,
            Brand = brand,
            IssuerBank = issuerBank
        };
    }

    public static PaymentValidationResult Fail(string error)
    {
        return new PaymentValidationResult
        {
            Success = false,
            Error = error
        };
    }
}
