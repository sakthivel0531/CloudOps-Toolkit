# Architecture

The CLI parses a local JSON object, evaluates deterministic rules, and prints text or JSON findings. Rule logic is separated from command-line I/O for testing.

Design principles: local-first, deterministic, testable, and explicit evidence/recommendations. The current input is an illustrative JSON schema, not an Azure Resource Manager template parser.
