using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TallyUp.Application.Dtos;
using TallyUp.Application.Interfaces;
using TallyUp.Controllers;
using Xunit;

namespace TallyUp.Tests.Controllers;

public class PollsControllerTests
{
    private readonly Mock<IPollService> _pollServiceMock;
    private readonly PollsController _controller;

    public PollsControllerTests()
    {
        _pollServiceMock = new Mock<IPollService>();
        _controller = new PollsController(_pollServiceMock.Object);
    }

    [Fact]
    public async Task GetPolls_ShouldReturn200WithPolls()
    {
        // Arrange
        var polls = new List<PollDto> { new() { Id = Guid.NewGuid(), Title = "Test Poll" } };
        _pollServiceMock.Setup(s => s.GetPollsAsync()).ReturnsAsync(polls);

        // Act
        var result = await _controller.GetPolls();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<PollDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(polls);
    }

    [Fact]
    public async Task GetPollById_ShouldReturn200WithPoll_WhenPollExists()
    {
        // Arrange
        var poll = new PollDto { Id = Guid.NewGuid(), Title = "Existing Poll" };
        _pollServiceMock.Setup(s => s.GetPollByIdAsync(poll.Id)).ReturnsAsync(poll);

        // Act
        var result = await _controller.GetPollById(poll.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PollDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(poll);
    }

    [Fact]
    public async Task GetPollById_ShouldReturn404_WhenPollDoesNotExist()
    {
        // Arrange
        _pollServiceMock.Setup(s => s.GetPollByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PollDto?)null);

        // Act
        var result = await _controller.GetPollById(Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<ActionResult<PollDto>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task CreatePoll_ShouldReturn201WithPoll()
    {
        // Arrange
        var createDto = new CreatePollDto { Title = "New Poll" };
        var createdPoll = new PollDto { Id = Guid.NewGuid(), Title = createDto.Title };
        _pollServiceMock.Setup(s => s.CreatePollAsync(createDto)).ReturnsAsync(createdPoll);

        // Act
        var result = await _controller.CreatePoll(createDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PollDto>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Value.Should().BeEquivalentTo(createdPoll);
    }

    [Fact]
    public async Task UpdatePoll_ShouldReturn200WithUpdatedPoll_WhenPollExists()
    {
        // Arrange
        var pollId = Guid.NewGuid();
        var updateDto = new UpdatePollDto { Title = "Updated Poll" };
        var updatedPoll = new PollDto { Id = pollId, Title = updateDto.Title };
        _pollServiceMock.Setup(s => s.UpdatePollAsync(pollId, updateDto)).ReturnsAsync(updatedPoll);

        // Act
        var result = await _controller.UpdatePoll(pollId, updateDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PollDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(updatedPoll);
    }

    [Fact]
    public async Task UpdatePoll_ShouldReturn404_WhenPollDoesNotExist()
    {
        // Arrange
        _pollServiceMock.Setup(s => s.UpdatePollAsync(It.IsAny<Guid>(), It.IsAny<UpdatePollDto>())).ReturnsAsync((PollDto?)null);

        // Act
        var result = await _controller.UpdatePoll(Guid.NewGuid(), new UpdatePollDto());

        // Assert
        var actionResult = Assert.IsType<ActionResult<PollDto>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task DeletePoll_ShouldReturn204_WhenPollExists()
    {
        // Arrange
        _pollServiceMock.Setup(s => s.DeletePollAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _controller.DeletePoll(Guid.NewGuid());

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeletePoll_ShouldReturn404_WhenPollDoesNotExist()
    {
        // Arrange
        _pollServiceMock.Setup(s => s.DeletePollAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _controller.DeletePoll(Guid.NewGuid());

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}