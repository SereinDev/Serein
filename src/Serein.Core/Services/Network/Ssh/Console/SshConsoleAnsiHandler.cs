using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Serein.Core.Utils;

namespace Serein.Core.Services.Network.Ssh.Console;

public static class SshConsoleAnsiHandler
{
    public static IEnumerable<ConsoleKeyInfo?> Handle(byte[] bytes)
    {
        if (bytes.Length == 0)
            return [];

        if (bytes[0] == '\x1b' && bytes.Length > 1)
            return HandleAsCSISequences(bytes);

        var text = EncodingMap.UTF8.GetString(bytes);

        throw new NotImplementedException();
    }

    private static IEnumerable<ConsoleKeyInfo?> HandleAsCSISequences(byte[] bytes)
    {
        if (bytes.Length < 3 || bytes[1] != '[')
            return [];

        var func = Convert.ToChar(bytes[^1]);
        ConsoleKeyInfo? info;

        switch (func)
        {
            case 'A':
            case 'B':
            case 'C':
            case 'D':
                info = new(
                    '\x00',
                    func switch
                    {
                        'A' => ConsoleKey.UpArrow,
                        _ => throw new NotSupportedException()
                    },
                    false,
                    false,
                    false
                );
                if (bytes.Length == 3)
                    return [info];

                if (bytes.Length == 4)
                    return Enumerable.Repeat(info, bytes[2]);

                break;

        }

        throw new NotImplementedException();
    }
}