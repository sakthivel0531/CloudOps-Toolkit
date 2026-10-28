# Changelog

All notable changes to this project are documented here.
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and the project uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
While the version is below 1.0, minor releases may include breaking changes; they will be called out here.

## [Unreleased]

### Added
- OpenSSF Scorecard job in the CI workflow, with results in the Security tab and a README badge.
- CodeQL scanning for C# and for the GitHub Actions workflow files.
- Dependency review on pull requests (fails on high or critical vulnerabilities).
- Vulnerable NuGet package check in the build job.
- Dependabot configuration for NuGet packages and GitHub Actions.
- Issue forms (bug report, feature request, new rule proposal), pull request template and CODEOWNERS.
- `ROADMAP.md`, this changelog, and `.github/release.yml` for generated release notes.

### Changed
- README rewritten with setup steps, usage examples, exit codes, input format, limitations and status badges.
- CI workflow now uses no default token permissions; each job requests only what it needs.
- GitHub Actions are pinned to full commit SHAs and updated to current major versions
  (`checkout` v7, `setup-dotnet` v6, `upload-artifact` v7).
- `SECURITY.md` and `CONTRIBUTING.md` expanded.

No changes to the CLI behavior or the existing tests.

## [0.1.0] - 2026-10-25 (initial commit, not tagged)

### Added
- Initial .NET 10 CLI: `check <path> [--format text|json]`.
- Baseline rules: CLOUD001 (public access), CLOUD002 (HTTPS-only disabled),
  CLOUD003 (legacy TLS), CLOUD004 (production resource without owner tag).
- xUnit tests, sample configuration, docs and CI build.

[Unreleased]: https://github.com/sakthivel0531/cloudops-toolkit/commits/main
[0.1.0]: https://github.com/sakthivel0531/cloudops-toolkit/commits/main
