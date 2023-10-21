namespace legend.Models.Address;

using System.ComponentModel.DataAnnotations;

public class AddAddressRequest
{
    [Required]
    public string StreetAddress { get; set; }
    public string ComplexBuilding { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Province { get; set; }
    [Required]
    public string Country { get; set; }
    public string Suburb { get; set; }
    [Required]
    public string PostalCode { get; set; }
    [Required]
    public Guid UserId { get; set; }
}