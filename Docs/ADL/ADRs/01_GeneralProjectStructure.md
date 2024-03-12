# Use Clean Architecture for the project structure

## Context and Problem Statement

It is beginning of the project and we need to define the project structure.
How to structure the project in a way that it is easy to understand and maintain?

## Considered Options

* Vertically structured project
* MVC structured project
* Clean Architecture structured project
* Big Ball of Everything

## Decision Outcome

Chosen option: "Clean Architecture structured project", because it is a good way to structure the project. 
By choosing this structure we are forcing ourself to separate business logic from more technical details, and data sources,
which might be helpful when project grows, and DDD or some splits of the project are needed.
