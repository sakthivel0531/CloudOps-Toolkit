# Security Policy

## Supported versions

The project is in early preview. Security fixes are made on the `main` branch and included in the next release.

| Version | Supported |
|---|---|
| `main` / latest release | Yes |
| Older releases | No |

## Reporting a vulnerability

Please **do not** open a public issue for a vulnerability.

1. Use GitHub's private reporting: **Security** tab → **Report a vulnerability**
   ([direct link](https://github.com/sakthivel0531/cloudops-toolkit/security/advisories/new)).
2. If that isn't available, contact the repository owner privately through a verified contact method on their GitHub profile.

Include the affected version or commit, steps to reproduce, and the impact you expect. This is a volunteer project, so responses are best effort; we aim to acknowledge reports within a week and will keep you updated until the issue is resolved. We're happy to credit reporters who want it.

## Scope

The toolkit reads local JSON files and does not need cloud credentials. Relevant reports include, for example, crashes or resource exhaustion caused by crafted input, or problems in the CI/CD workflows or dependencies. Findings produced by the rules are guidance, not a compliance certification; a rule that misses a misconfiguration is a bug or feature request rather than a vulnerability.

## How the project protects itself

- **Least-privilege CI:** the workflow token has no default permissions; each job requests only what it needs, and checkouts don't persist credentials.
- **Pinned actions:** third-party GitHub Actions are pinned to full commit SHAs.
- **Automated updates:** Dependabot opens weekly PRs for NuGet packages and GitHub Actions.
- **Scanning:** CodeQL (C# and workflow files), dependency review on pull requests, a vulnerable NuGet package check, and OpenSSF Scorecard.

## For contributors

Never commit tokens, credentials, private keys, or unredacted production configuration. Use sanitized sample data in issues, tests and examples.
