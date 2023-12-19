using Serein.Core.Models.OneBot.Packets;

using RegexMatch = System.Text.RegularExpressions.Match;

namespace Serein.Core.Models.Commands;

public record CommandContext(RegexMatch? Match = null, MessagePacket? MessagePacket = null);