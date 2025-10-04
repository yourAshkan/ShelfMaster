using ShelfMaster.Domain.Enums;

namespace ShelfMaster.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public UserRole Role { get; set; }
}
