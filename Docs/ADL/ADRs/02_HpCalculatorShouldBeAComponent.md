# Calculating Hp should be encapsulated in a separate component

## Context and Problem Statement

HP Calculating capabilites are needed in the project. It need to be placed somewhere in solution sturcutre
we need to think about coupling and cohesion of the solution.

## Considered Options

* Hp Calculation is part of the functionality of the character sheet, so it should be a feature in character sheet
  component
* HP calucation should be a separate component, because it has its own logic

## Decision Outcome

Chosen option: "HP calculation should be a separate component", because big part of character sheet opeations are still
to be defined,
and in that way we can concentrate on the defined business logic. We should be aware that in future this decision can
change and this component can be merged to the main component of application.