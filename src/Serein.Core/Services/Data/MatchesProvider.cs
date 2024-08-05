using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class MatchesProvider : DataProviderBase<ObservableCollection<Match>>
{

    public MatchesProvider()
    {
        Value = [];
        Read();
    }

    public override ObservableCollection<Match> Value { get; }

    public override ObservableCollection<Match> Read()
    {
        try
        {
            if (File.Exists(PathConstants.MatchesFile))
            {
                var wrapper = JsonSerializer.Deserialize<DataItemWrapper<List<Match>>>(
                    File.ReadAllText(PathConstants.MatchesFile),
                    JsonSerializerOptionsFactory.CamelCase
                );

                if (wrapper?.Type == typeof(ObservableCollection<Match>).ToString())
                    lock (Value)
                    {
                        Value.Clear();

                        if (wrapper.Data is not null)
                            foreach (var match in wrapper.Data)
                            {
                                Value.Add(match);
                            }
                    }
            }
            else
                Save();

            return Value;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"加载匹配文件（{PathConstants.MatchesFile}）时出现异常", e);
        }
    }

    public override void Save()
    {
        try
        {
            Directory.CreateDirectory(PathConstants.Root);
            File.WriteAllText(
                PathConstants.MatchesFile,
                JsonSerializer.Serialize(
                    DataItemWrapper.Wrap(Value),
                    options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
                )
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"保存匹配文件（{PathConstants.MatchesFile}）时出现异常", e);
        }
    }
}
