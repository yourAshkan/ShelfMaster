using ShelfMaster.Domain.Enums;

namespace ShelfMaster.Application.DTOs;

public class LoanDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    //public string? UserFirstName { get; set; }
    //public string? UserLastName { get; set; }
    public int? BookId { get; set; }
    public string? BookTitle { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsDeleted { get; set; } = false;
}
