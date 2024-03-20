# Project should use SQLite as a Database

## Context and Problem Statement
We need a database to store and manage our data. The database should be easy to deploy and flexible to accommodate changes in the data structure.

## Considered Options
1. SQL Server
2. MySQL
3. PostgreSQL
4. SQLite

## Decision Outcome
Chosen option: "SQLite", because it is easy to deploy and does not require connection strings or an external database for project inspection. It is a file-based database, which means it can be deployed anywhere.

### Pros of SQLite
- Portability: SQLite is a file-based database, which means it can be deployed anywhere. This makes it a great choice for development, testing, and even small production applications.
- No need for connection strings or external databases: SQLite does not require a connection string or an external database for project inspection. This simplifies the setup process and makes it easier to get started.
- Document database approach: We decided to use SQLite in a "document database" fashion because our data structure is not final yet. This approach will make it easier to change the stored data later on without heavy migrations.

### Cons of SQLite
- Not suitable for large-scale applications: SQLite may not be the best choice for large-scale applications with high concurrency or write-intensive operations.
- Limited functionality: SQLite lacks some of the features and functionality provided by more robust databases like SQL Server or PostgreSQL.

## Related ADRs