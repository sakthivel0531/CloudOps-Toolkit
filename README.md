# CloudOps Toolkit

[![Build and test](https://github.com/sakthivel0531/CloudOps-Toolkit/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/sakthivel0531/CloudOps-Toolkit/actions/workflows/ci.yml)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/sakthivel0531/CloudOps-Toolkit/badge)](https://scorecard.dev/viewer/?uri=github.com/sakthivel0531/CloudOps-Toolkit)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
![Status: early preview](https://img.shields.io/badge/status-early%20preview-orange)

A small .NET 10 command-line tool that checks a cloud resource configuration (JSON) against a set of baseline rules and reports findings with evidence and a recommendation.

> **Status: early preview (v0.1).** The toolkit analyzes **local JSON files only**. It does not connect to Azure or any cloud provider, and it does not verify deployed resources. See [Limitations](#limitations).

## Contents

- [Quick start](#quick-start)
- [Setup](#setup)
- [Usage](#usage)
- [Examples](#examples)
- [Rules](#rules)
- [Limitations](#limitations)
- [Project layout](#project-layout)
- [Contributing, roadmap and releases](#contributing-roadmap-and-releases)
- [Security](#security)

## Quick start

```bash
git clone https://github.com/sakthivel0531/CloudOps-Toolkit.git
cd cloudops-toolkit
dotnet run --project src/CloudOps.Toolkit -- check examples/sample-config.json
```

The sample file is intentionally misconfigured, so this prints four findings and exits with code `1`.

## Setup

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (10.0.100 or a later feature band; see `global.json`)
- Git

Check your SDK:

```bash
dotnet --version   # should print 10.0.x
```

### Build and test

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

### Publish a standalone executable (optional)

```bash
dotnet publish src/CloudOps.Toolkit --configuration Release --output ./out
./out/CloudOps.Toolkit check examples/sample-config.json
```

On Windows, run `.\out\CloudOps.Toolkit.exe` instead.

## Usage

```text
cloudops check <path-to-json> [--format text|json]
```

| Option | Description |
|---|---|
| `check <path>` | Analyze one JSON file describing a single resource. |
| `--format text` | Human-readable output (default). |
| `--format json` | Machine-readable output for scripts and pipelines. |
| `--help`, `-h` | Show usage. |

### Exit codes

| Code | Meaning |
|---|---|
| `0` | No `ERROR` findings (warnings may still be reported). |
| `1` | At least one `ERROR` finding. |
| `2` | Invalid command, unsupported format, missing file or invalid JSON. |

### Input format

The input is one JSON object. These properties are read; anything else is ignored:

| Property | Type | Used by |
|---|---|---|
| `publicAccess` | boolean | CLOUD001 |
| `httpsOnly` | boolean | CLOUD002 |
| `minimumTlsVersion` | string | CLOUD003 |
| `environment` | string | CLOUD004 |
| `tags.owner` | string | CLOUD004 |

## Examples

### Text output

```bash
dotnet run --project src/CloudOps.Toolkit -- check examples/sample-config.json
```

```text
[ERROR] CLOUD001: Public access is enabled
  Evidence: 'publicAccess' is true.
  Recommendation: Disable public access unless explicitly required and approved.
[ERROR] CLOUD002: HTTPS-only access is disabled
  Evidence: 'httpsOnly' is false.
  Recommendation: Require HTTPS where supported and verify TLS configuration.
[ERROR] CLOUD003: Legacy TLS version configured
  Evidence: 'minimumTlsVersion' is 'TLS1_0'.
  Recommendation: Use a currently supported TLS version consistent with service capabilities and policy.
[WARNING] CLOUD004: Production resource has no owner tag
  Evidence: No non-empty 'tags.owner' value was found.
  Recommendation: Add an owner tag for operational responsibility and cost allocation.
```

### A compliant configuration

```json
{
  "resourceName": "demo-storage",
  "publicAccess": false,
  "httpsOnly": true,
  "minimumTlsVersion": "TLS1_2",
  "environment": "production",
  "tags": { "owner": "platform-team" }
}
```

```text
No findings from enabled baseline rules.
```

### JSON output

```bash
dotnet run --project src/CloudOps.Toolkit -- check examples/sample-config.json --format json
```

Returns an array of findings (shortened here). Single quotes are escaped as `'` by the default .NET serializer:

```json
[
  {
    "RuleId": "CLOUD001",
    "Severity": "ERROR",
    "Title": "Public access is enabled",
    "Evidence": "'publicAccess' is true.",
    "Recommendation": "Disable public access unless explicitly required and approved."
  }
]
```

### Use it in a CI pipeline

The exit code makes it easy to fail a build on `ERROR` findings:

```yaml
- name: Check resource configuration
  run: dotnet run --project src/CloudOps.Toolkit --configuration Release -- check config/storage.json
```

To keep the report but not fail the job, capture JSON and decide later:

```bash
dotnet run --project src/CloudOps.Toolkit -- check config/storage.json --format json > findings.json || true
```

## Rules

| Rule | Severity | Triggers when |
|---|---|---|
| `CLOUD001` | ERROR | `publicAccess` is `true` |
| `CLOUD002` | ERROR | `httpsOnly` is `false` |
| `CLOUD003` | ERROR | `minimumTlsVersion` is TLS 1.0 or TLS 1.1 |
| `CLOUD004` | WARNING | `environment` is `production` and `tags.owner` is missing or empty |

Details are in [docs/security-checks.md](docs/security-checks.md). Want a new rule? Use the **New rule proposal** issue template.

## Limitations

- **Local JSON only.** No cloud API calls, no credentials, no live resource inspection.
- **Illustrative schema.** The input is a simplified format, not an ARM template, Bicep file, Terraform plan or Azure Resource Graph export.
- **One resource per file.** The root must be a JSON object; arrays of resources are not supported yet.
- **Four generic rules.** This is not a complete security assessment and not a compliance certification. Validate findings against current provider documentation and your organization's policy.
- **Missing properties are not findings.** If a property such as `httpsOnly` is absent, its rule does not fire.
- **Simple argument parsing.** Unknown options are ignored, and `--format` without a value falls back to text.
- **No packaged release yet.** Run from source or `dotnet publish`; there is no NuGet tool package or prebuilt binary.

## Project layout

```text
src/CloudOps.Toolkit/            CLI entry point (Program.cs) and rules (BaselineRules.cs)
tests/CloudOps.Toolkit.Tests/    xUnit tests for the rules
examples/                        Sample input
docs/                            Architecture, getting started and rule docs
.github/workflows/ci.yml         Build, test, CodeQL, dependency review and OpenSSF Scorecard
.github/ISSUE_TEMPLATE/          Bug, feature and rule-proposal forms
```

## Contributing, roadmap and releases

- [CONTRIBUTING.md](CONTRIBUTING.md): how to set up, test and open a pull request
- [ROADMAP.md](ROADMAP.md): what's planned next
- [CHANGELOG.md](CHANGELOG.md): release notes
- [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)

Good first contributions: new rules with tests, clearer docs, and sample inputs.

## Security

CI runs CodeQL (C# and workflow files), dependency review on pull requests, a vulnerable-package check, and OpenSSF Scorecard. Dependabot keeps NuGet packages and pinned GitHub Actions up to date.

Please report vulnerabilities privately. See [SECURITY.md](SECURITY.md).

## License

[MIT](LICENSE)
