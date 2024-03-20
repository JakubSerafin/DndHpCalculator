# Data access should be wrapped in a repository

## Context and Problem Statement

From decision
[Each API Endpoint sholud have Automatic testing of happy path for each endpoint with mocked data](03_EachEndpointShouldHaveHappyPathTested.md)
we know that we need
to have a way to mock data for testing. This lead to build a system that allow to swap data sources.

## Considered Options

* Data access should be wrapped in a repository
* Data access should be an ORM with onMemory database for testing

## Decision Outcome

Chosen option: "Data access should be wrapped in a repository". This allow to future elasticity about data sources
and to mock data for testing. Outcomes of this decision are:

* We can easily swap data sources
* In future more extensive mapping might be needed between data source and domain model

## Releated ADR-s

Reason for this decision:
[Each API Endpoint sholud have Automatic testing of happy path for each endpoint with mocked data](03_EachEndpointShouldHaveHappyPathTested.md)