using ShelfMaster.Domain.Entities;

namespace ShelfMaster.Domain.IRepository;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int id);
    Task<List<Loan>> GetAllAsync();
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task DeleteAsync(int id);
    Task<List<Loan?>> GetAllByUserIdAsync(int id);
}
