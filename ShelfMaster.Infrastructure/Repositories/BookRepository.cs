using Microsoft.EntityFrameworkCore;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.IRepository;
using ShelfMaster.Infrastructure.DbContext;

namespace ShelfMaster.Infrastructure.Repositories;

public class BookRepository(AppDbContext _context) : IBookRepository
{
    #region Add
    public async Task AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    } 
    #endregion

    #region Update
    public async Task UpdateAsync(Book book)
    {
        var bookId = book.Id;
        if (bookId <= 0)
            throw new Exception("Book Not Found!");

        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    } 
    #endregion

    #region Delete
    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            book.SoftDeleted();
            await _context.SaveChangesAsync();
        }
    } 
    #endregion

    #region GetAll
    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    } 
    #endregion

    #region GetById
    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    } 
    #endregion
}
