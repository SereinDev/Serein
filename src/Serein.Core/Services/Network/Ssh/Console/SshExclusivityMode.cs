using System;
using System.Threading.Tasks;

using Spectre.Console;

namespace Serein.Core.Services.Network.Ssh.Console;

public class SshExclusivityMode : IExclusivityMode
{
    public T Run<T>(Func<T> func)
    {
        return func();
    }

    public Task<T> RunAsync<T>(Func<Task<T>> func)
    {
        return func();
    }
}