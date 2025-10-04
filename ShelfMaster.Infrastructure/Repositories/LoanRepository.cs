using Microsoft.EntityFrameworkCore;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.IRepository;
using ShelfMaster.Infrastructure.DbContext;

namespace ShelfMaster.Infrastructure.Repositories;

public class LoanRepository(AppDbContext _context) : ILoanRepository

{
    #region Add
    public async Task AddAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Update
    public async Task UpdateAsync(Loan loan)
    {
        var loanid = loan.Id;
        if (loanid <= 0)
            throw new Exception("Loan Not Found!");

        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    } 
    #endregion

    #region Delete
    public async Task DeleteAsync(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan != null)
        {
            loan.SoftDeleted();
            await _context.SaveChangesAsync();
        }
    }
    #endregion

    #region GetAll
    public async Task<List<Loan>> GetAllAsync()
    {
        return await _context.Loans
            .Include(x=>x.UserId)
            .Include(x=> x.BookId)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }
    #endregion

    #region GetById
    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _context.Loans
            .Include(x=>x.UserId)
            .Include(x=>x.BookId)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }
    #endregion
}
