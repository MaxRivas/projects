<!--
Guidance for AI coding agents (GitHub Copilot / copilot-cli) working in MoviesApp.
Keep this file short, specific, and actionable. Update via `.specify/scripts/bash/update-agent-context.sh` when project plans change.
-->

# Copilot instructions — MoviesApp (concise)

- Purpose: provide immediate, project-aware guidance for automated code edits, PR suggestions, and small feature scaffolds.
- Always read and honor the project constitution at `.specify/memory/constitution.md` before making policy or workflow changes.

## What to know about this repo

- Project layout conventions (see `.specify/templates/plan-template.md`):
  - Single-project: `src/`, `tests/`
  - Web app: `backend/src/`, `frontend/src/`
  - Mobile: `api/src/`, `ios/src/` or `android/src/`
- Agent context and auto-generated guidance live under `.specify/` (templates, prompts, and scripts).

## Where to write or update agent-visible docs

- Auto-generated agent files use template `.specify/templates/agent-file-template.md`.
- The update script is `.specify/scripts/bash/update-agent-context.sh` — run it (or update templates) when you change tech stack or commands.
- Preserve manual additions in generated files between the markers:
  `<!-- MANUAL ADDITIONS START -->` and `<!-- MANUAL ADDITIONS END -->`.

## Code & style expectations (actionable rules)

- Prefer the repository conventions in `.specify/templates/tasks-template.md` and language examples found in templates (tests in `tests/`, models in `src/models/`, services in `src/services/`).
- Do not change project structure without updating `.specify/memory/constitution.md` and the agent-file template.

## Editing rules for Copilot suggestions

- When creating or updating an agent context file (e.g., this file), include:
  - Active Technologies (from plan files)
  - Project Structure snippet (use the `Project Structure` fenced block in the agent template)
  - Commands only for detected technologies (do not invent unfamiliar build/test commands)
- If you cannot determine the language or commands from plan files, leave a TODO with the exact file and field, e.g. `TODO(COMMANDS): determine test commands from plan.md`.

## Example file paths to reference

- Constitution template: `.specify/memory/constitution.md`
- Agent template: `.specify/templates/agent-file-template.md`
- Update script: `.specify/scripts/bash/update-agent-context.sh`
- Plan templates: `.specify/templates/plan-template.md`, `.specify/templates/tasks-template.md`

## Quick-run (for maintainers)

Run the updater script to refresh agent docs after changing `.specify/*` or plan files:

```bash
./.specify/scripts/bash/update-agent-context.sh copilot
```

## Safety & scope

- Avoid making large design changes. Propose edits as a minimal patch and include an explicit migration plan when changing APIs or project structure.
- Preserve existing comments and manual sections unless a maintainer asks you to refactor them.

---
If anything in these instructions is unclear or you want more examples from the codebase, ask before applying large changes.
