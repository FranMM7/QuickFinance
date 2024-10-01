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
  - [Summary Endpoints](#summary-endpoints)
- [Contributing](#contributing)
- [License](#license)

## Features
- **Manage Expenses:** Add, update, view, and delete your expenses.
- **Budgeting:** Set budgets for different categories and track your spending.
- **Categories:** Organize your expenses into categories for better management.
- **Payment Methods:** Track different payment methods for your expenses.
- **User-Friendly API:** RESTful API design for easy integration and access.

## Technologies Used
- **Backend:** ASP.NET Core 8.0 (with plans for an alternative backend using Laravel)
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
   git clone https://github.com/frankmejia7/QuickFinance.git
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
   - [http://localhost:5000/api/categories/summary](http://localhost:5000/api/categories/summary)

## API Endpoints

### Expenses
- **GET** `/api/expenses`: Retrieve all expenses
- **POST** `/api/expenses`: Create a new expense
- **PUT** `/api/expenses/{id}`: Update an existing expense
- **DELETE** `/api/expenses/{id}`: Delete an expense

### Categories
- **GET** `/api/categories`: Retrieve all categories
- **POST** `/api/categories`: Create a new category
- **PUT** `/api/categories/{id}`: Update an existing category
- **DELETE** `/api/categories/{id}`: Delete a category

### Budgets
- **GET** `/api/budgets`: Retrieve all budgets
- **POST** `/api/budgets`: Create a new budget
- **PUT** `/api/budgets/{id}`: Update an existing budget
- **DELETE** `/api/budgets/{id}`: Delete a budget

### Payment Methods
- **GET** `/api/paymentmethods`: Retrieve all payment methods
- **POST** `/api/paymentmethods`: Create a new payment method
- **PUT** `/api/paymentmethods/{id}`: Update an existing payment method
- **DELETE** `/api/paymentmethods/{id}`: Delete a payment method

### Summary Endpoints
- **GET** `/api/expenses/summary`: Retrieve a summary of all expenses (queried directly from a stored procedure)
- **GET** `/api/categories/summary`: Retrieve a summary of all categories (queried directly from a stored procedure)
- **GET** `/api/budgets/summary`: Retrieve a summary of all budgets (queried directly from a stored procedure)

## Contributing
Contributions are welcome! If you have suggestions or improvements, feel free to create an issue or submit a pull request.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
