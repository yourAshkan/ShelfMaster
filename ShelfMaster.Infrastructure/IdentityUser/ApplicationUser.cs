using Microsoft.AspNetCore.Identity;

namespace ShelfMaster.Infrastructure.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
