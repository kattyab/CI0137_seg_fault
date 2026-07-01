namespace Backend.Api.Services;

public class PaymentBinInfo
{
    public string BankName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;

    public PaymentBinInfo(string bankName, string brand)
    {
        BankName = bankName;
        Brand = brand;
    }
}
