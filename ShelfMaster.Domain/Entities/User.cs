using ShelfMaster.Domain.Enums;

namespace ShelfMaster.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string NationalCode { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public UserRole Role { get; set; }

    public List<Loan>? Loan { get; set; }

    public User()
    {

    }

    public void UpdateProfile(string newfirstname, string newlastname, string newemail, string phonenumber)
    {
        FirstName = newfirstname;
        LastName = newlastname;
        PhoneNumber = phonenumber;
        Email = newemail;
    }
}
