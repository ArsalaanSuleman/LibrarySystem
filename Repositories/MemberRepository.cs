namespace LibrarySystem.Repositories;

using Microsoft.EntityFrameworkCore;
using LibrarySystem.Data;
using LibrarySystem.Interfaces;
using LibrarySystem.Entities;

public class EfMemberRepository : IMemberRepository
{
    private readonly LibraryContext _context;

    public EfMemberRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<Member>> GetAllAsync()
    {
        // Include() ber EF Core om å hente ActiveLoans samtidig (SQL JOIN)
        // Uten Include() ville ActiveLoans vært en tom liste
        return await _context.Members
            .Include(m => m.ActiveLoans)
            .ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _context.Members
            .Include(m => m.ActiveLoans)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(Member member)
    {
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();
    }
}