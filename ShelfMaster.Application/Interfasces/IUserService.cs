namespace ShelfMaster.Application.Interfasces;

public interface IUserService
{
    Task<string?> GetGetUserNameByIdAsync(int id);
}
