using System;

namespace TallyUp.Domain.Entities;

public class Poll
{
    public Guid Id { get; set; } 
    public string Title { get; set; } 
    public string Description { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime? UpdatedAt { get; set; }  
}