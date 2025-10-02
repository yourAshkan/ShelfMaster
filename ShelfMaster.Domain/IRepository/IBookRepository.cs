using ShelfMaster.Domain.Entities;

namespace ShelfMaster.Domain.IRepository;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> GetAllAsync();
    Task AddAsync(Book book);
    Task DeleteAsync(int id);
    Task UpdateAsync (Book book);
}
