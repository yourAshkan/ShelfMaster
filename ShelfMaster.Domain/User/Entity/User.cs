namespace ShelfMaster.Domain.User.Entity;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Family { get; private set; }
    public string NationalCode { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Email { get; private set; }

    public User()
    {

    }
}
