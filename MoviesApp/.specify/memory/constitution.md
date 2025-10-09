# MoviesApp Constitution

## Core Principles

### I. Code Quality (MUST)
All production code MUST be clear, well-named, and small. Functions should do one thing and be under ~150 lines where practical. Public interfaces MUST include concise docstrings or JSDoc comments explaining inputs, outputs, and error modes. Follow the repository layout in `.specify/templates/tasks-template.md` (models → `src/models/`, services → `src/services/`, endpoints → `src/`).

Rationale: Small, documented units speed review and make automated testing reliable.

### II. Test-First and Deterministic Tests (MUST)
Every feature MUST include automated tests before or alongside implementation. Unit tests belong next to source (e.g., `tests/` or `src/**/tests/`). Tests MUST be deterministic (no sleeping, randomness without seeding, or reliance on external network services). Use mocks/fakes for external systems and keep integration tests separate and slower.

Acceptance: Pull requests that add or change behavior MUST include passing tests exercising the new behavior and at least one negative/error case.

### III. Integration & Contract Testing (SHOULD)
Areas where different components interact (backend ↔ frontend, service ↔ DB) SHOULD have contract-style integration tests that validate schema and surface-level behavior. Integration tests may run in CI as a separate job and may use lightweight containers or in-memory test doubles.

### IV. User Experience Consistency (MUST)
API responses and CLI/UIs MUST be consistent: error payloads should include `code`, `message`, and optional `details`. Frontend components (if present under `frontend/src/`) MUST follow existing naming and folder conventions; reuse common UI components rather than creating duplicates.

Example: Error JSON: {"code": "NOT_FOUND", "message": "Movie not found", "details": {"id": "123"}}

### V. Performance & Resource Budget (SHOULD)
New endpoints or features SHOULD document expected performance characteristics in the associated spec or task (average response time, 95th percentile SLA, memory/CPU expectations). Avoid loading large datasets into memory; prefer streaming/pagination for list endpoints.

Baseline: small feature endpoints should aim for <250ms median response on modest hardware; document deviations explicitly.

## Additional Constraints

- Use the `.specify/templates/plan-template.md` to infer project structure. Do not rearrange top-level directories without a documented migration plan.
- Preserve manual additions in auto-generated agent files between `<!-- MANUAL ADDITIONS START -->` and `<!-- MANUAL ADDITIONS END -->`.

## Development Workflow & Quality Gates

- Local checks: run unit tests and linters before opening a PR. Example commands are gathered into agent files by `.specify/scripts/bash/update-agent-context.sh`.
- CI gates MUST include at minimum: install → unit tests → lint. Integration tests may be a separate pipeline job.
- Code review: PRs must include a short description, link to spec/plan, and which tests were added/changed.

## Governance

Constitution amendments require updating `.specify/memory/constitution.md` and following the `speckit.constitution` flow (see `.github/prompts/speckit.constitution.prompt.md`). Version must follow semantic increment rules. If a principle is changed in a way that affects templates, run `.specify/scripts/bash/update-agent-context.sh` and verify generated agent files.

**Version**: 1.0.0 | **Ratified**: 2025-10-09 | **Last Amended**: 2025-10-09