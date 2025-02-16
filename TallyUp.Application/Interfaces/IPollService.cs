
using TallyUp.Application.Dtos;

namespace TallyUp.Application.Interfaces;

public interface IPollService
{
    Task<IEnumerable<PollDto>> GetPollsAsync();
    Task<PollDto?> GetPollByIdAsync(Guid id);
    Task<PollDto> CreatePollAsync(CreatePollDto pollDto);
    Task<PollDto?> UpdatePollAsync(Guid id, UpdatePollDto pollDto);
    Task<bool> DeletePollAsync(Guid id);
}