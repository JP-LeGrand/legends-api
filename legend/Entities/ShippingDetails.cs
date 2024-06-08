using System.ComponentModel.DataAnnotations.Schema;

namespace legend.Entities
{
    public class ShippingDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ShippingDetailsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? ComplexBuilding { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public string? Suburb { get; set; }
        public string? PostalCode { get; set; }
    }

}
