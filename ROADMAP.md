# Roadmap

This roadmap shows direction, not commitments. Priorities can change based on feedback.
Have an idea? Open a feature request or a new rule proposal.

## Now: v0.1.x (stabilize the preview)

- [x] Baseline rules CLOUD001 to CLOUD004 with tests
- [x] CI with build, test, CodeQL, dependency review and OpenSSF Scorecard
- [x] Dependabot for NuGet and GitHub Actions
- [ ] Add tests for the CLI itself (exit codes, `--format`, error messages)
- [ ] Readable JSON output (no `'` escaping) and camelCase property names, behind an option to avoid breaking scripts
- [ ] Reject unknown options and a `--format` with no value instead of ignoring them

## Next: v0.2 (more useful input and output)

- [ ] Analyze an array of resources, or a folder of JSON files, in one run
- [ ] SARIF output so findings show up in GitHub code scanning
- [ ] `--fail-on warning|error` to control the exit code
- [ ] Include the resource name in each finding
- [ ] Publish as a .NET global tool (`dotnet tool install`)

## Later: v0.3 and beyond (ideas)

- [ ] Resource-type-specific rules (storage, web apps, key vaults)
- [ ] Read ARM template or Bicep build output
- [ ] Rule suppression with a required justification
- [ ] Signed release binaries with build provenance

## Out of scope for now

- Connecting to live cloud accounts or requiring credentials
- Acting as a compliance certification tool
