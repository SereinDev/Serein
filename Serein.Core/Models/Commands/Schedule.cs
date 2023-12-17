namespace Serein.Core.Models.Commands;

public class Schedule
{
    public string? CronExp { get; set; }

    public string? Command { get; set; }

    public string? Description { get; set; }

    public bool Enable { get; set; }
}
