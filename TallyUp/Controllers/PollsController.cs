using Microsoft.AspNetCore.Mvc;
using TallyUp.Application.Dtos;
using TallyUp.Application.Interfaces;
using TallyUp.Domain.Constants;
using TallyUp.Filters;

namespace TallyUp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;

    public PollsController(IPollService pollService)
    {
        _pollService = pollService;
    }

    [HttpGet]
    [HasPermission(Permissions.ReadPoll)]
    [ProducesResponseType(typeof(IEnumerable<PollDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PollDto>>> GetPolls()
    {
        var polls = await _pollService.GetPollsAsync();
        return Ok(polls);
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.ReadPoll)]
    [ProducesResponseType(typeof(PollDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PollDto>> GetPollById(Guid id)
    {
        var poll = await _pollService.GetPollByIdAsync(id);
        if (poll == null) 
            return NotFound(new { message = "Poll not found" });

        return Ok(poll);
    }

    [HttpPost]
    [HasPermission(Permissions.CreatePoll)]
    [ProducesResponseType(typeof(PollDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PollDto>> CreatePoll([FromBody] CreatePollDto pollDto)
    {
        var createdPoll = await _pollService.CreatePollAsync(pollDto);
        return CreatedAtAction(nameof(GetPollById), new { id = createdPoll.Id }, createdPoll);
    }

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.EditPoll)]
    [ProducesResponseType(typeof(PollDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PollDto>> UpdatePoll(Guid id, [FromBody] UpdatePollDto pollDto)
    {
        var updatedPoll = await _pollService.UpdatePollAsync(id, pollDto);
        if (updatedPoll == null) 
            return NotFound(new { message = "Poll not found" });

        return Ok(updatedPoll);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.DeletePoll)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePoll(Guid id)
    {
        var result = await _pollService.DeletePollAsync(id);
        if (!result) 
            return NotFound(new { message = "Poll not found" });

        return NoContent();
    }
}
