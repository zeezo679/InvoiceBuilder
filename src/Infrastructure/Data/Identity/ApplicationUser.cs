using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastLoginAt { get; set; }
    public string ProfileImageUrl { get; set; }
}