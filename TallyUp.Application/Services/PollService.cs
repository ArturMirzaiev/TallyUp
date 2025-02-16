using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallyUp.Application.Dtos;
using TallyUp.Application.Interfaces;
using TallyUp.Domain.Entities;
using TallyUp.Domain.Interfaces;

namespace TallyUp.Application.Services;

public class PollService : IPollService
{
    private readonly IPollRepository _pollRepository;
    private readonly IMapper _mapper;

    public PollService(IPollRepository pollRepository, IMapper mapper)
    {
        _pollRepository = pollRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PollDto>> GetPollsAsync()
    {
        var polls = await _pollRepository.GetPolls().ToListAsync(); 
        return _mapper.Map<IEnumerable<PollDto>>(polls);
    }

    public async Task<PollDto?> GetPollByIdAsync(Guid id)
    {
        var poll = await _pollRepository.GetPollByIdAsync(id);
        return poll != null ? _mapper.Map<PollDto>(poll) : null;
    }

    public async Task<PollDto> CreatePollAsync(CreatePollDto pollDto)
    {
        var poll = _mapper.Map<Poll>(pollDto);
        await _pollRepository.AddPollAsync(poll);
        await _pollRepository.SaveChangesAsync();  // Теперь явно сохраняем изменения
        return _mapper.Map<PollDto>(poll);
    }

    public async Task<PollDto?> UpdatePollAsync(Guid id, UpdatePollDto pollDto)
    {
        var existingPoll = await _pollRepository.GetPollByIdAsync(id);
        if (existingPoll == null) return null;

        _mapper.Map(pollDto, existingPoll);
        _pollRepository.UpdatePoll(existingPoll);
        await _pollRepository.SaveChangesAsync();  // Теперь явно сохраняем изменения

        return _mapper.Map<PollDto>(existingPoll);
    }

    public async Task<bool> DeletePollAsync(Guid id)
    {
        var poll = await _pollRepository.GetPollByIdAsync(id);
        if (poll == null) return false;

        await _pollRepository.DeletePollAsync(id);
        await _pollRepository.SaveChangesAsync();  // Теперь явно сохраняем изменения
        return true;
    }
}