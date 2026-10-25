using System.Text.Json;
using CloudOps.Toolkit;
using Xunit;

namespace CloudOps.Toolkit.Tests;

public sealed class BaselineRulesTests
{
    [Fact]
    public void ReportsPublicAccessAndHttpsDisabled()
    {
        using var doc = JsonDocument.Parse("{\"publicAccess\":true,\"httpsOnly\":false}");
        var findings = BaselineRules.Analyze(doc.RootElement);
        Assert.Contains(findings, f => f.RuleId == "CLOUD001");
        Assert.Contains(findings, f => f.RuleId == "CLOUD002");
    }

    [Theory]
    [InlineData("TLS1_0")]
    [InlineData("TLS1_1")]
    [InlineData("TLS 1.0")]
    [InlineData("TLS 1.1")]
    public void ReportsLegacyTls(string tls)
    {
        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(new { minimumTlsVersion = tls }));
        Assert.Contains(BaselineRules.Analyze(doc.RootElement), f => f.RuleId == "CLOUD003");
    }

    [Fact]
    public void WarnsForProductionWithoutOwner()
    {
        using var doc = JsonDocument.Parse("{\"environment\":\"production\",\"tags\":{}}");
        Assert.Contains(BaselineRules.Analyze(doc.RootElement), f => f.RuleId == "CLOUD004");
    }

    [Fact]
    public void DoesNotWarnWhenOwnerIsPresent()
    {
        using var doc = JsonDocument.Parse("{\"environment\":\"production\",\"tags\":{\"owner\":\"platform\"}}");
        Assert.DoesNotContain(BaselineRules.Analyze(doc.RootElement), f => f.RuleId == "CLOUD004");
    }

    [Fact]
    public void ReturnsNoFindingsForSafeSample()
    {
        using var doc = JsonDocument.Parse("{\"publicAccess\":false,\"httpsOnly\":true,\"minimumTlsVersion\":\"TLS1_2\",\"environment\":\"dev\"}");
        Assert.Empty(BaselineRules.Analyze(doc.RootElement));
    }
}
