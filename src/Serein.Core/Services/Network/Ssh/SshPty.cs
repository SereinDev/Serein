using System;
using System.Threading;

using FxSsh.Services;

// using Serein.Core.Services.Network.Ssh.Console;
using Serein.Core.Utils;

namespace Serein.Core.Services.Network.Ssh;

public class SshPty
{
    private const char EscapeCode = '\u001b';
    private const string ResetCode = "c";
    private const string ClearCode = "[2J";
    private const string LinewrapEnableCode = "[7h";
    private const string LinewrapDisableCode = "[7l";
    private const string CursorHomeCode = "[H";
    private const string CursorPosCode = "[{0};{1}H";
    private const string CursorShowCode = "[?25h";
    private const string CursorHideCode = "[?25l";
    private const string CursorUpCode = "[{0}A";
    private const string CursorDownCode = "[{0}B";
    private const string CursorForwardCode = "[{0}C";
    private const string CursorBackwardCode = "[{0}D";

    private bool _readingKey;

    public byte[]? LastKeys { get; private set; }

    public SshPty(SessionChannel sessionChannel, string terminal)
    {
        Terminal = terminal;
        Channel = sessionChannel;

    }

    public string Terminal { get; set; }

    public uint WidthChars { get; set; }

    public uint HeightChars { get; set; }

    public uint WidthPx { get; set; }

    public uint HeightPx { get; set; }

    public SessionChannel Channel { get; }

    public void EnableLinewrap()
    {
        SendCommand(LinewrapEnableCode);
    }

    public void DisableLinewrap()
    {
        SendCommand(LinewrapDisableCode);
    }

    public void ShowCursor()
    {
        SendCommand(CursorShowCode);
    }

    public void HideCursor()
    {
        SendCommand(CursorHideCode);
    }

    public void SetCursor(int x, int y)
    {
        if (x == 1 && y == 1)
        {
            SendCommand(CursorHomeCode);

            return;
        }

        SendCommand(string.Format(CursorPosCode, y, x));
    }

    // public void MoveCursor(CursorDirection direction, int steps)
    // {
    //     SendCommand(string.Format(direction switch
    //     {
    //         CursorDirection.Left => CursorBackwardCode,
    //         CursorDirection.Right => CursorForwardCode,
    //         CursorDirection.Up => CursorUpCode,
    //         CursorDirection.Down => CursorDownCode,
    //         _ => throw new NotSupportedException()
    //     }, steps));
    // }

    public void Send(string text)
    {
        Channel.SendData(B(text));
    }

    public void Reset()
    {
        SendCommand(ResetCode);
    }

    public void Clear()
    {
        Channel.SendData(B($"{EscapeCode + CursorHomeCode + EscapeCode + ClearCode}"));
    }

    private void SendCommand(string command)
    {
        Channel.SendData(C(command));
    }

    public void BeginReadKey(EventWaitHandle eventWaitHandle)
    {
        if (_readingKey)
            throw new InvalidOperationException();

        LastKeys = null;
        _readingKey = true;

        Channel.DataReceived += Wait;

        void Wait(object? sender, byte[] bytes)
        {
            Channel.DataReceived -= Wait;
            eventWaitHandle.Set();
            _readingKey = false;
            LastKeys = bytes;
        }
    }

    private static byte[] C(string code)
    {
        return B(EscapeCode + code);
    }

    private static byte[] B(string text)
    {
        return EncodingMap.UTF8.GetBytes(text);
    }

}