namespace ShelfMaster.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsAdmin { get; set; }
}
