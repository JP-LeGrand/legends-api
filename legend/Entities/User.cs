namespace legend.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }

    public List<Address> DeliveryAddresses { get; set; }
}