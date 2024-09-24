namespace legend.Models.Users;

using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    private string _username;
    public string Username
    {
        get { return _username; }
        private set { _username = EmailAddress; }
    }

    [Required]
    public string Password { get; set; }

    private string _emailAddress;

    [Required]
    public string EmailAddress
    {
        get { return _emailAddress; }
        set
        {
            _emailAddress = value;
            Username = value;
        }
    }

    [Required]
    public string PhoneNumber { get; set; }
}