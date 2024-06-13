using System;

using Octokit;

namespace Serein.Core.Models.Network;

public class UpdateEventArgs(Release release) : EventArgs
{
    public Release Release { get; set; } = release;

    public DateTime Time { get; } = DateTime.Now;
}
