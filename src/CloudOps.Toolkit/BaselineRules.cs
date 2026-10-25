using System.Text.Json;

namespace CloudOps.Toolkit;

public sealed record Finding(string RuleId, string Severity, string Title, string Evidence, string Recommendation);

public static class BaselineRules
{
    public static IReadOnlyList<Finding> Analyze(JsonElement resource)
    {
        var findings = new List<Finding>();

        if (GetBool(resource, "publicAccess") is true)
            findings.Add(new("CLOUD001", "ERROR", "Public access is enabled",
                "'publicAccess' is true.",
                "Disable public access unless explicitly required and approved."));

        if (GetBool(resource, "httpsOnly") is false)
            findings.Add(new("CLOUD002", "ERROR", "HTTPS-only access is disabled",
                "'httpsOnly' is false.",
                "Require HTTPS where supported and verify TLS configuration."));

        var tls = GetString(resource, "minimumTlsVersion");
        if (tls is not null && new[] { "TLS1_0", "TLS1_1", "TLS 1.0", "TLS 1.1" }
            .Contains(tls, StringComparer.OrdinalIgnoreCase))
            findings.Add(new("CLOUD003", "ERROR", "Legacy TLS version configured",
                $"'minimumTlsVersion' is '{tls}'.",
                "Use a currently supported TLS version consistent with service capabilities and policy."));

        if (string.Equals(GetString(resource, "environment"), "production", StringComparison.OrdinalIgnoreCase)
            && string.IsNullOrWhiteSpace(GetOwner(resource)))
            findings.Add(new("CLOUD004", "WARNING", "Production resource has no owner tag",
                "No non-empty 'tags.owner' value was found.",
                "Add an owner tag for operational responsibility and cost allocation."));

        return findings;
    }

    private static bool? GetBool(JsonElement element, string name)
    {
        if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out var p))
        {
            if (p.ValueKind == JsonValueKind.True) return true;
            if (p.ValueKind == JsonValueKind.False) return false;
        }
        return null;
    }

    private static string? GetString(JsonElement element, string name) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(name, out var p) &&
        p.ValueKind == JsonValueKind.String ? p.GetString() : null;

    private static string? GetOwner(JsonElement resource)
    {
        if (resource.ValueKind != JsonValueKind.Object ||
            !resource.TryGetProperty("tags", out var tags) ||
            tags.ValueKind != JsonValueKind.Object)
            return null;
        return GetString(tags, "owner");
    }
}
