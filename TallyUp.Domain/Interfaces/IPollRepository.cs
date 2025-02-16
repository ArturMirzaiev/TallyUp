using TallyUp.Domain.Entities;

namespace TallyUp.Domain.Interfaces;

public interface IPollRepository
{
    IQueryable<Poll> GetPolls();
    Task<Poll?> GetPollByIdAsync(Guid id);
    Task AddPollAsync(Poll poll);
    void UpdatePoll(Poll poll);
    Task DeletePollAsync(Guid id);
    Task SaveChangesAsync();  // Новый метод
}