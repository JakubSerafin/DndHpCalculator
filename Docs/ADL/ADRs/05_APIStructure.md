# Data access should be wrapped in a repository
## Context and Problem Statement
API should have hava structure based on requiremets of the project.

## Considered Options

## Decision Outcome
API should have following structure:
```
- CharacterSheet
    - Create
    - Read
    - List
- HpCalculator
    - Create
    - Read
    - List
    - Delete
    - Update
```
Character sheet endpoint should serve as a main endpoint for the project, but also be as shalow as posible, because managing
character sheet is not a main goal of the project at current stage. HP Calculator should have all CRUD operations.

## Releated ADR-s
- [HP Calculation](ADRs/09_HpCalculation.md)