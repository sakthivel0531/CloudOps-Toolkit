# Contributing

Thanks for contributing! Small, focused pull requests are easiest to review.

## Set up

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
2. Fork and clone the repository.
3. Run `dotnet restore` and `dotnet test`.

## Before you start

- Check [open issues](https://github.com/sakthivel0531/cloudops-toolkit/issues) and the [roadmap](ROADMAP.md).
- For anything bigger than a small fix, open an issue first using one of the templates (bug report, feature request or new rule proposal) so the approach can be agreed.

## Making a change

1. Keep pull requests focused and explain the problem being solved.
2. Add tests for behavior changes and update docs/examples as needed.
3. For a new rule: use the next free `CLOUDxxx` ID, add it to `BaselineRules.cs`, add tests that cover a triggering and a non-triggering input, and document it in `docs/security-checks.md` and the README rules table.
4. Add a line under **Unreleased** in [CHANGELOG.md](CHANGELOG.md).
5. Never commit credentials, production exports, personal data, or secrets.
6. Ensure build and tests pass before opening a pull request:

   ```bash
   dotnet build --configuration Release
   dotnet test --configuration Release
   ```

CI also runs CodeQL, dependency review and OpenSSF Scorecard. Please fix any new alerts your change introduces.

## Reporting bugs

Use the **Bug report** issue form. Provide a minimal sanitized JSON example, the command, expected behavior, actual output and exit code. Report security problems privately as described in [SECURITY.md](SECURITY.md).

## Releases (maintainers)

1. Move the **Unreleased** entries in `CHANGELOG.md` under a new version heading.
2. Tag the commit (`git tag v0.x.y && git push origin v0.x.y`).
3. Create a GitHub release from the tag and click **Generate release notes**; categories come from `.github/release.yml` and PR labels.
