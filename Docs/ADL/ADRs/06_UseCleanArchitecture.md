# Project should use Clean Architecture

## Context and Problem Statement
How to structure the project to be easily maintainable and testable.
## Considered Options
There are many standard patterns to structure a project: 
- MVC
- Hexagonal
- Clean Architecture
- Onion Architecture
- Ports and Adapters

## Decision Outcome

Chosen option: Clean Architecture. This architecture is chosen because it provide nice folders/namespaces
structure to the solution, but also separate all business logic to the core of the application. This allow to easily
test the business logic and to swap data sources.

## Releated ADR-s