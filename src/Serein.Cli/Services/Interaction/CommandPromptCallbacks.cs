using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using PrettyPrompt;
using PrettyPrompt.Completion;
using PrettyPrompt.Documents;

using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public partial class CommandPromptCallbacks : PromptCallbacks
{
    private readonly CommandProvider _commandProvider;
    private readonly ServerManager _serverManager;

    public CommandPromptCallbacks(CommandProvider commandProvider, ServerManager serverManager)
    {
        _commandProvider = commandProvider;
        _serverManager = serverManager;
    }

    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(
        string text,
        int caret,
        TextSpan spanToBeReplaced,
        CancellationToken cancellationToken
    )
    {
        var typedWord = text.AsSpan(spanToBeReplaced.Start, spanToBeReplaced.Length).ToString();
        var args = text.Split(
            '\x20',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );
        if (args.Length <= 1) // 根命令
            return Task.FromResult<IReadOnlyList<CompletionItem>>(
                [
                    .. _commandProvider
                        .RootCommandItems.OrderByDescending(
                            (item) => CalculateRelevance(item.ReplacementText, typedWord)
                        )
                        .ThenBy((item) => item.ReplacementText[0]),
                ]
            );

        if (args.Length > 0)
            switch (args[0])
            {
                case "connection" when args.Length == 2:
                    return Task.FromResult<IReadOnlyList<CompletionItem>>(
                        [
                            .. _connectionSubcommnads.OrderByDescending(
                                (item) => CalculateRelevance(item.ReplacementText, typedWord)
                            ),
                        ]
                    );

                case "server" when args.Length == 2:
                    return Task.FromResult<IReadOnlyList<CompletionItem>>(
                        [
                            .. GetServerCompletionItem()
                                .OrderByDescending(
                                    (item) => CalculateRelevance(item.ReplacementText, typedWord)
                                )
                                .ThenBy((item) => item.ReplacementText[0]),
                        ]
                    );

                case "server" when args.Length == 3:
                    return Task.FromResult<IReadOnlyList<CompletionItem>>(
                        [
                            .. _serverSubcommnads
                                .OrderByDescending(
                                    (item) => CalculateRelevance(item.ReplacementText, typedWord)
                                )
                                .ThenBy((item) => item.ReplacementText[0]),
                        ]
                    );
            }

        return Task.FromResult<IReadOnlyList<CompletionItem>>([]);
    }

    private static int CalculateRelevance(string word, string input)
    {
        for (int i = word.Length; i > 0; i--)
            if (input == word[..i])
                return i;
            else if (input.Contains(word[..i]))
                return i - 10;

        return int.MinValue;
    }
}
