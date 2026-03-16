namespace LibrarySystem.Interfaces;

using LibrarySystem.Entities;
    
public interface IMemberRepository
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task AddAsync(Member member);
}