namespace Backend.Api.Services;

public class PaymentGatewayService
{
    public PaymentGatewayResult Authorize(string cardNumber, decimal amount)
    {
        string digits = new string(cardNumber.Where(char.IsDigit).ToArray());

        if (digits.EndsWith(PaymentConstants.DeclinedCardSuffix))
        {
            return PaymentGatewayResult.Reject("La transacción fue rechazada por el banco emisor.");
        }

        if (amount <= PaymentConstants.MinimumTransactionAmount)
        {
            return PaymentGatewayResult.Reject("El monto de la transacción no es válido.");
        }

        if (amount > PaymentConstants.MaximumTransactionAmount)
        {
            return PaymentGatewayResult.Reject("El monto supera el límite permitido.");
        }

        string authorizationCode = Guid.NewGuid()
            .ToString("N")[..PaymentConstants.AuthorizationCodeLength]
            .ToUpper();

        return PaymentGatewayResult.Approve($"AUTH-{authorizationCode}");
    }
}

public class PaymentGatewayResult
{
    public bool IsApproved { get; set; }
    public string? AuthorizationCode { get; set; }
    public string? RejectionReason { get; set; }

    public static PaymentGatewayResult Approve(string authorizationCode)
    {
        return new PaymentGatewayResult
        {
            IsApproved = true,
            AuthorizationCode = authorizationCode
        };
    }

    public static PaymentGatewayResult Reject(string reason)
    {
        return new PaymentGatewayResult
        {
            IsApproved = false,
            RejectionReason = reason
        };
    }
}
