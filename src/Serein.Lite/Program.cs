using System;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Lite.Ui;
using Serein.Lite.Ui.Function;
using Serein.Lite.Ui.Servers;

namespace Serein.Lite;

public static class Program
{
    private static readonly SereinApp App;

    static Program()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<ISereinLogger, LiteLogger>();

        builder.Services.AddSingleton<MainForm>();
        builder.Services.AddSingleton<ServerPage>();
        builder.Services.AddSingleton<MatchPage>();
        builder.Services.AddSingleton<ConnectionPage>();

        App = builder.Build();
    }

    [STAThread]
    public static void Main()
    {
        App.StartAsync();

        ApplicationConfiguration.Initialize();
        Application.Run(App.Services.GetRequiredService<MainForm>());
    }
}
