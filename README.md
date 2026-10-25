# CloudOps Toolkit

An initial .NET 10 CLI project for analyzing sample cloud configuration JSON and reporting baseline findings.

> This is a reference implementation. It analyzes local JSON only; it does not connect to Azure or verify deployed resources.

## Requirements
- .NET 10 SDK

## Build and test
```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

## Run
```bash
dotnet run --project src/CloudOps.Toolkit -- check examples/sample-config.json
dotnet run --project src/CloudOps.Toolkit -- check examples/sample-config.json --format json
```

Exit codes: `0` no error findings, `1` error-severity findings, `2` invalid command/input.

## Initial rules
- `CLOUD001`: public access enabled
- `CLOUD002`: HTTPS-only disabled
- `CLOUD003`: legacy TLS version specified
- `CLOUD004`: production resource has no owner tag

These generic rules are illustrative, not a complete Azure security or compliance assessment. Validate findings against current service documentation and organizational policy.

## Layout
- `src/CloudOps.Toolkit/` CLI and rules
- `tests/CloudOps.Toolkit.Tests/` automated tests
- `examples/` sample input
- `docs/` design and rule documentation
- `.github/workflows/ci.yml` CI

See [CONTRIBUTING.md](CONTRIBUTING.md) and [SECURITY.md](SECURITY.md).
