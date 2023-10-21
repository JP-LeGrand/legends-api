namespace legend.Models.Address;

public class UpdateAddressRequest
{
    public string StreetAddress { get; set; }

    public string ComplexBuilding { get; set; }

    public string City { get; set; }

    public string Province { get; set; }

    public string Country { get; set; }

    public string Suburb { get; set; }

    public string PostalCode { get; set; }

    public Guid UserId { get; set; }
}