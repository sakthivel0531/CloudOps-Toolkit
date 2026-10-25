using System.Text.Json;

if (args.Length == 0 || args[0] is "--help" or "-h" or "help")
{
    Console.WriteLine("CloudOps Toolkit");
    Console.WriteLine("Usage: cloudops check <path-to-json> [--format text|json]");
    return;
}

if (!string.Equals(args[0], "check", StringComparison.OrdinalIgnoreCase) || args.Length < 2)
{
    Console.Error.WriteLine("Invalid command. Use: cloudops check <path-to-json> [--format text|json]");
    Environment.ExitCode = 2;
    return;
}

var path = args[1];
var format = "text";
for (var i = 2; i < args.Length; i++)
{
    if (args[i] == "--format" && i + 1 < args.Length)
        format = args[++i].ToLowerInvariant();
}

if (format is not ("text" or "json"))
{
    Console.Error.WriteLine("Unsupported format. Choose 'text' or 'json'.");
    Environment.ExitCode = 2;
    return;
}

try
{
    using var document = JsonDocument.Parse(await File.ReadAllTextAsync(path));
    if (document.RootElement.ValueKind != JsonValueKind.Object)
    {
        Console.Error.WriteLine("Input must be a JSON object.");
        Environment.ExitCode = 2;
        return;
    }

    var findings = CloudOps.Toolkit.BaselineRules.Analyze(document.RootElement);
    if (format == "json")
        Console.WriteLine(JsonSerializer.Serialize(findings, new JsonSerializerOptions { WriteIndented = true }));
    else if (findings.Count == 0)
        Console.WriteLine("No findings from enabled baseline rules.");
    else
        foreach (var f in findings)
        {
            Console.WriteLine($"[{f.Severity}] {f.RuleId}: {f.Title}");
            Console.WriteLine($"  Evidence: {f.Evidence}");
            Console.WriteLine($"  Recommendation: {f.Recommendation}");
        }

    Environment.ExitCode = findings.Any(f => f.Severity == "ERROR") ? 1 : 0;
}
catch (FileNotFoundException)
{
    Console.Error.WriteLine($"Input file not found: {path}");
    Environment.ExitCode = 2;
}
catch (JsonException ex)
{
    Console.Error.WriteLine($"Invalid JSON: {ex.Message}");
    Environment.ExitCode = 2;
}
catch (IOException ex)
{
    Console.Error.WriteLine($"Could not read input file: {ex.Message}");
    Environment.ExitCode = 2;
}
