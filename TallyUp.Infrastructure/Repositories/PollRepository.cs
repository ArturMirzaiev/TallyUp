using Microsoft.EntityFrameworkCore;
using TallyUp.Domain.Entities;
using TallyUp.Domain.Interfaces;
using TallyUp.Infrastructure.Data;

namespace TallyUp.Infrastructure.Repositories;

public class PollRepository : IPollRepository
{
    private readonly ApplicationDbContext _context;

    public PollRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Poll> GetPolls()
    {
        return _context.Polls.AsQueryable();
    }

    public async Task<Poll?> GetPollByIdAsync(Guid id)
    {
        return await _context.Polls.FindAsync(id);
    }

    public async Task AddPollAsync(Poll poll)
    {
        await _context.Polls.AddAsync(poll);
    }

    public void UpdatePoll(Poll poll)
    {
        _context.Polls.Update(poll);
    }

    public async Task DeletePollAsync(Guid id)
    {
        var poll = await _context.Polls.FindAsync(id);
        if (poll != null)
        {
            _context.Polls.Remove(poll);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}