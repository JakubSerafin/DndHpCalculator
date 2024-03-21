# DnD Test Assignment
Welcome to the DnD Test Assignment. This project demonstrates my approach to problem-solving in DnD Beyond's back-end developer challenge.

## Problem
The problem is described here:
https://github.com/DnDBeyond/ddb-back-end-developer-challenge

## Solution
### Architecture of the Solution
The repository is based on Clean Architecture and consists of 3 layers:
- Domain: This layer stores the business logic and entities and does not depend on any other layer. It calculates and stores the current player's HP.
- Controller: This layer defines the API endpoints and the request/response models. It also contains data models used as input/output formats by the API.
- Infrastructure: This layer implements the repositories and code needed to connect to external services/data sources. It also contains data models used by these sources.
- Application: (Not implemented in the current solution)

### Authorization
The application uses basic authorization with an API Key. The API Key is stored in the `appsettings.json` file and is used to access operations restricted to the Game Master, such as:
- Creating a new character
- All operations related to modifying a character's HP

### Testing
The application uses xUnit for testing. Currently, only integration tests are implemented. These tests use in-memory repositories.

### Database
To simplify local project setup and review, SQLite is used as the database. It stores characters and their HP. The DB file is created in the root of the project and is named `DnD.db`.

### Reasoning Behind the Architecture and Decisions
The reasoning behind the architecture and decisions can be found here:
[ADL.MD](Docs%2FADL%2FADL.MD)

## How to Run
### Prerequisites
-  A computer
- .NET Core 8.0

### Steps
1. Clone the repository
2. Open the terminal and navigate to the root of the repository
3. Set the API Key in the `appsettings.json` file
4. Run the `dotnet restore` command to restore the dependencies
5. Run the solution using the `dotnet run` command
6. Open a browser and navigate to `https://localhost:5001/swagger/index.html` to view the API documentation
7. You're all set!

## Next Steps
There are several potential next steps for this project:
- Switch to a more secure authorization method like OpenID
- Move the data to a more secure database like SQL Server
- Introduce user management and allow each user to see only their characters
- Add more tests
- Automatically detect and update changes in defenses and statistics based on inventory and equipment
- Add more operations to the API
- Implement a description feature in HpModificators to allow the Game Master to add descriptions to each modification. Currently, the description is just a placeholder.
- Swagger schema could be improved to provide more information about the API
- Better bad request handling - more meaningful error messages