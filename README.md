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