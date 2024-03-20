# Each API Endpoint sholud have Automatic testing of happy path for each endpoint with mocked data

## Context and Problem Statement

Context: Start of new project
Problem: how to ensure application rotbustness and ensure that old functionality is not broken by new changes?

## Considered Options

* Manual testing
* Unit testing with some percentage of coverage
* Automatic testing of happy path for each endpoint with mocked data
* End to testing of each endpoint with real data

## Decision Outcome

Chosen option: "Automatic testing of happy path for each endpoint with mocked data", because it is a good way to ensure
that application is not broken by new changes,
and it is good compromise between time of execution and quality of the application.
This approach also force architecture to allow seed the data to aplication