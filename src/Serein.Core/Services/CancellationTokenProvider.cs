using System.Threading;

namespace Serein.Core.Services;

public sealed class CancellationTokenProvider
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public CancellationToken Token => _cancellationTokenSource.Token;

    internal void Cancel()
    {
        if (!Token.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
