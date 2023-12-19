using System.Text.RegularExpressions;

var match = Regex.Match(
    "[abc:111:1]123",
    @"^\[(?<name>[a-zA-Z]+)(:(?<argument>[\w\-\s]+))?\](?<body>.+)$"
);
Console.WriteLine(match.Success);
Console.WriteLine(string.Join(',', match.Groups.Keys));
Console.WriteLine(match.Groups["name"]);
Console.WriteLine(match.Groups["argument"].Value);
Console.WriteLine(match.Groups["argument"].Value == "");
Console.WriteLine(match.Groups["body"]);
