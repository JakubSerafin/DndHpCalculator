# DnD Test Assigment 
Welcome to the DnD Test Assigment. This is a simple test to see how I approached a problem and solve it.


## Problem
Was described here:
https://github.com/DnDBeyond/ddb-back-end-developer-challenge

## Solution
### Architecture of solution
Repository is based on the Clean Architecture. It has 3 layers:
- Domain: Storing the business logic and the entities, should not depend on any other layer. Here current player HP is calculated and stored.
- Controller: Storing the API endpoints and the request/response models. Here the API endpoints are defined. Also, this layer contains data models to be used by an API as input/oputput format
- Infrastructure: Storing the implementation of the repositories and code neede to connect to external services/data sources. Also contains data models to be used by that sources
- Application: (Not implemented in current solution)

### Authorization 
Aplication is using basic authorization with API Key. Api key is stored in the appsettings.json file, and it is used to access opetation restricted for Game Master like:
- Create a new character
- All operations related to modification of character HP

### Testing
Application is using xUnit for testing. Right now only integration tests are implemented. In integration tests, in memory repositories are used

### Database 
To make project more easy to run and check locally, SQLite is used as a database. It is used to store characters and their HP. DB File is created in the root of the project and it is named `DnD.db`


### Reasoning behind the architecture and decision 
Some of the reasons behind the architecture and decision are here:
[ADL.MD](Docs%2FADL%2FADL.MD)

## How to run 
### Prerequisites
-  Computer
- .NET Core 8.0

### Steps
1. Clone the repository
2. Open the terminal and navigate to the root of the repository
3. set the API Key in the appsettings.json file
4. run `dotnet restore` command to restore the dependencies
5. run solution using `dotnet run` command
6. Open the browser and navigate to `https://localhost:5001/swagger/index.html` to see the API documentation
7. Yaay! You are ready to go


## Next steps
Of coures there are some next steps to be done. Some of them are:
- Switch to some more secure authorization method like OpenID
- Move the data to some more secure database like SQL Server
- Introduce user management and allow each user to see only his characters
- Add more tests
- Automatically detect and update changes in defences and statistic based on the inventory and equipment
- Add more operations to the API