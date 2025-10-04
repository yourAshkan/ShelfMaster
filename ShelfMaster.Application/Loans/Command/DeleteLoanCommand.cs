using MediatR;

namespace ShelfMaster.Application.Loans.Command;

public class DeleteLoanCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}
