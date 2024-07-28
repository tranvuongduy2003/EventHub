namespace EventHub.Domain.DTOs.Payment;

public class CreateBankCardDto
{
    public string AddressCity { get; set; }

    public string AddressCountry { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string AddressState { get; set; }

    public string AddressZip { get; set; }

    public string Cvc { get; set; }

    public long ExpMonth { get; set; }

    public long ExpYear { get; set; }

    public string Name { get; set; }

    public string Number { get; set; }
}