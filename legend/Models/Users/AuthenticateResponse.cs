namespace legend.Models.Users;

public class AuthenticateResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Token { get; set; }
}