```markdown
# QuickFinance

QuickFinance is a simple personal finance tracker that allows users to manage their expenses, budgets, categories, and payment methods efficiently. This application is designed to help users keep track of their financial activities and gain insights into their spending habits.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [API Endpoints](#api-endpoints)
  - [Expenses](#expenses)
  - [Categories](#categories)
  - [Budgets](#budgets)
  - [Payment Methods](#payment-methods)
  - [Finance Evaluation](#finance-evaluation)
  - [Location](#location)
  - [Shopping](#shopping)
  - [General](#general)
- [Contributing](#contributing)
- [License](#license)

## Features
- **Manage Expenses:** Add, update, view, and delete your expenses.
- **Budgeting:** Set budgets for different categories and track your spending.
- **Categories:** Organize your expenses into categories for better management.
- **Payment Methods:** Track different payment methods for your expenses.
- **Finance Evaluation:** Evaluate your financial situation based on various parameters.
- **Shopping:** Manage your shopping lists and their states.
- **Locations:** Track locations for expense management.
- **User-Friendly API:** RESTful API design for easy integration and access.

## Technologies Used
- **Backend:** ASP.NET Core 8.0
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Frontend:** Vue.js, TypeScript, Vite, Bootstrap, FontAwesome

## Getting Started

### Prerequisites
Before you begin, ensure you have met the following requirements:
- .NET 8.0 SDK
- SQL Server or any compatible database server
- A code editor like Visual Studio or Visual Studio Code

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/franmm7/QuickFinance.git
   cd QuickFinance
   ```
2. Restore the dependencies:
   ```bash
   dotnet restore
   ```
3. Update the connection string in `appsettings.json` to point to your database:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=QuickFinanceDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
   }
   ```
4. Run migrations to set up the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```
6. Access the API: Open your browser and navigate to the following endpoints:
   - [http://localhost:5000/api/expenses](http://localhost:5000/api/expenses)
   - [http://localhost:5000/api/categories](http://localhost:5000/api/categories)
   - [http://localhost:5000/api/budgets](http://localhost:5000/api/budgets)
   - [http://localhost:5000/api/paymentmethods](http://localhost:5000/api/paymentmethods)
   - [http://localhost:5000/api/financeevaluation](http://localhost:5000/api/financeevaluation)
   - [http://localhost:5000/api/location](http://localhost:5000/api/location)
   - [http://localhost:5000/api/shopping](http://localhost:5000/api/shopping)
   - [http://localhost:5000/api/general](http://localhost:5000/api/general)

7. Configure `appsettings.json`

In the root of the project, locate or create the `appsettings.json` file and add the following configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=DBInstanceName;Database=DBName;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "AllowedHosts": "*"
}
```
DBInstanceName: Replace this with the name of your SQL Server instance (e.g., localhost, SQLEXPRESS, or a custom instance name).
DBName: Replace this with the name of the database for QuickFinance (e.g., QuickFinanceDB).
Make sure to use correct credentials or configurations if connecting to a remote or secured database

### Explanation of `appsettings.json` Configuration

In the `appsettings.json` file, you’ll find key configurations that help control various aspects of the application:

- **Logging**: Configures how the application logs information, warnings, errors, and more.
  - `"LogLevel"`: Sets the logging level, which defines the minimum severity of messages that will be logged. 
    - `"Default": "Information"`: This setting will log informational messages, warnings, and errors. 
    - `"Microsoft.AspNetCore": "Warning"`: This setting limits logging for ASP.NET Core framework-specific messages to warnings and errors only. This keeps the logs cleaner by excluding lower-level framework information.

- **ConnectionStrings**: Stores the details required to connect the application to a database.
  - `"DefaultConnection"`: This is the main connection string the application uses to access the database.
    - **Server**: The name of the SQL Server instance (e.g., `localhost`, `SQLEXPRESS`) where the database is hosted.
    - **Database**: The name of the database (e.g., `QuickFinanceDB`) that will store application data.
    - **Trusted_Connection=True**: Uses the Windows authentication of the current user to connect to the database (useful in local development).
    - **TrustServerCertificate=True**: Allows the application to connect without verifying the server's SSL certificate (common in local development; use caution in production).
    - **MultipleActiveResultSets=True**: Enables multiple active queries on a single connection, which is useful when using an ORM like Entity Framework.

- **AllowedHosts**: Specifies which hosts are allowed to connect to the application.
  - `"*"`: The wildcard `*` allows requests from any host, which is typically acceptable in development. In production, you can specify specific domain names or IP addresses to restrict access (e.g., `"AllowedHosts": "mydomain.com"`).

These configurations allow the application to log important events, securely connect to the database, and control host access. Understanding each of these will help you troubleshoot issues, optimize connections, and secure the app in different environments.

## API Endpoints

### **Expenses**
- **GET** `/api/expenses`: Retrieve all expenses.
- **POST** `/api/expenses`: Create a new expense.
- **PUT** `/api/expenses/{id}`: Update an existing expense.
- **DELETE** `/api/expenses/{id}`: Delete an expense.

### **Categories**
- **GET** `/api/categories`: Retrieve all categories.
- **POST** `/api/categories`: Create a new category.
- **PUT** `/api/categories/{id}`: Update an existing category.
- **DELETE** `/api/categories/{id}`: Delete a category.
- **GET** `/api/categories/CategoriesType`: Retrieve available category types.
- **PUT** `/api/categories/ChangeState`: Change the state of a category.

### **Budgets**
- **GET** `/api/budgets`: Retrieve all budgets.
- **POST** `/api/budgets`: Create a new budget.
- **PUT** `/api/budgets/{id}`: Update an existing budget.
- **DELETE** `/api/budgets/{id}`: Delete a budget.
- **GET** `/api/budgets/BudgetsInfo`: Retrieve general budget info.
- **PUT** `/api/budgets/ChangeState`: Change the state of a budget.

### **Payment Methods**
- **GET** `/api/paymentmethods`: Retrieve all payment methods.
- **POST** `/api/paymentmethods`: Create a new payment method.
- **PUT** `/api/paymentmethods/{id}`: Update an existing payment method.
- **DELETE** `/api/paymentmethods/{id}`: Delete a payment method.

### **Finance Evaluation**
- **GET** `/api/FinanceEvaluation/List`: Retrieve a list of finance evaluations.
- **GET** `/api/FinanceEvaluation`: Retrieve finance evaluation details.
- **POST** `/api/FinanceEvaluation`: Create a new finance evaluation.
- **GET** `/api/FinanceEvaluation/{id}`: Retrieve a specific finance evaluation by its ID.
- **PUT** `/api/FinanceEvaluation/{id}`: Update a specific finance evaluation by its ID.
- **DELETE** `/api/FinanceEvaluation/{id}`: Delete a specific finance evaluation by its ID.
- **PUT** `/api/FinanceEvaluation/ChangeState`: Change the state of a finance evaluation.

### **Location**
- **GET** `/api/Location/List`: Retrieve all locations.
- **GET** `/api/Location/{id}`: Retrieve a specific location by its ID.
- **PUT** `/api/Location/{id}`: Update a location.
- **DELETE** `/api/Location/{id}`: Delete a location.
- **POST** `/api/Location`: Create a new location.
- **PUT** `/api/Location/ChangeState`: Change the state of a location.

### **Shopping**
- **GET** `/api/Shopping`: Retrieve shopping records.
- **POST** `/api/Shopping`: Create a new shopping record.
- **GET** `/api/Shopping/List`: Retrieve a list of shopping records.
- **GET** `/api/Shopping/Clone`: Clone an existing shopping record.
- **GET** `/api/Shopping/{id}`: Retrieve a specific shopping record by its ID.
- **PUT** `/api/Shopping/{id}`: Update a shopping record.
- **DELETE** `/api/Shopping/{id}`: Delete a shopping record.
- **PUT** `/api/Shopping/ChangeState`: Change the state of a shopping record.

### **General**
- **GET** `/api/General/TotalPages`: Fetch pagination information for various tables.
```