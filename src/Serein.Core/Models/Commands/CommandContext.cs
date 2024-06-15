using System.Collections.Generic;

using Serein.Core.Models.Network.Connection.OneBot.Packets;

using RegexMatch = System.Text.RegularExpressions.Match;

namespace Serein.Core.Models.Commands;

public record CommandContext(
    RegexMatch? Match = null,
    MessagePacket? MessagePacket = null,
    IReadOnlyDictionary<string, string?>? Variables = null
);
