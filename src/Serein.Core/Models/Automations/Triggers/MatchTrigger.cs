using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Serein.Core.Models.Automations.Triggers;

public sealed class MatchTrigger : TriggerBase
{
    private string _pattern = string.Empty;
    private bool _isRegex;

    public MatchFieldType FieldType { get; set; } = MatchFieldType.ServerOutput;

    public string Pattern
    {
        get => _pattern;
        set
        {
            _pattern = value;
            CompileRegex();
        }
    }

    public bool IsRegex
    {
        get => _isRegex;
        set
        {
            _isRegex = value;
            CompileRegex();
        }
    }

    [JsonIgnore]
    public Regex? Regex { get; private set; }

    private void CompileRegex()
    {
        if (IsRegex && !string.IsNullOrEmpty(Pattern))
        {
            try
            {
                Regex = new Regex(Pattern);
            }
            catch
            {
                Regex = null;
            }
        }
        else
        {
            Regex = null;
        }
    }
}
