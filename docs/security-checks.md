# Baseline rules

| Rule | Severity | Condition |
|---|---|---|
| CLOUD001 | ERROR | `publicAccess` is `true` |
| CLOUD002 | ERROR | `httpsOnly` is `false` |
| CLOUD003 | ERROR | `minimumTlsVersion` is TLS 1.0 or TLS 1.1 |
| CLOUD004 | WARNING | Production environment has no non-empty `tags.owner` |

These generic checks are not a full Azure security assessment or compliance certification. Resource-specific settings vary; verify results against current service documentation and organizational policy.
