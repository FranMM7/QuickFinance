```markdown
# QuickFinance Frontend

## Overview

QuickFinance is a personal finance tracking application that helps users manage their budgets, expenses, and categories efficiently. This frontend application is built using Vue.js, Bootstrap, and Vite for a responsive and interactive user experience.

## Features

- User-friendly interface for managing budgets and expenses.
- Display and edit categories and expenses.
- Responsive design with Bootstrap styling.
- Option for users to select a theme, including light and dark modes.
- Ability to clone records and edit their details.
- Responsive design with Bootstrap and custom Bootswatch themes.
- Axios for API calls to the backend.
- State management using pinia.

## Tech Stack

- **Vue.js**: A progressive JavaScript framework for building user interfaces.
- **Bootstrap 5**: A popular CSS framework for developing responsive and mobile-first web pages.
- **Bootswatch**: Free themes for Bootstrap.
- **Vite**: A modern frontend build tool for faster development and hot module replacement.
- **Axios**: A promise-based HTTP client for making API requests.
- **Pinia**: State management library for Vue.js.
- **Vue Router**: The official router for Vue.js to manage navigation.
- **Vue i18n**: Internationalization plugin for Vue.js to handle multiple languages.
- **FontAwesome**: A popular icon library for scalable vector icons.
- **Vue Toastification**: A library for toast notifications in Vue.js applications.
- **Vitest**: A fast testing framework for Vue.js projects.
- **Prettier**: An opinionated code formatter for maintaining consistent code style.
- **ESLint**: A tool for identifying and fixing problems in JavaScript code.
- **TypeScript**: A typed superset of JavaScript that compiles to plain JavaScript for enhanced developer experience and reliability.
- **Vite Plugin Vue DevTools**: A plugin for better debugging and development tools integration.
- **Vue Content Loader**: A component for creating SVG-based loading skeletons.


## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/franmm7/QuickFinance-Frontend.git
   cd QuickFinance-Frontend
   ```

2. Install the dependencies:

   ```bash
   npm install
   ```

3. Start the development server:

   ```bash
   npm run dev
   ```

4. Open your browser and visit `http://localhost:8080`.

## Usage

- Navigate through the application using the menu.
- Use the **Budgets**, **Categories**, **Finance Analysis**, and **Shopping** sections to manage your financial data.
- Use the **Settings** section to configure your preferences, 
- Use the **Theme** for selecting your theme (light or dark mode and others).
- Users can now clone records with a simple click, which will allow them to edit the cloned data.
- Each record has a "pencil" edit icon that appears on hover to allow editing the description field directly.

## API Endpoints

### **Budgets**

- **GET** `/api/Budgets/BudgetsInfo`: Fetch general information about budgets.
- **GET** `/api/Budgets/List`: Fetch a list of all budgets.
- **GET** `/api/Budgets/{id}`: Fetch a specific budget by its ID.
- **PUT** `/api/Budgets/{id}`: Update a specific budget by its ID.
- **DELETE** `/api/Budgets/{id}`: Delete a specific budget by its ID.
- **POST** `/api/Budgets`: Create a new budget.
- **PUT** `/api/Budgets/ChangeState`: Change the state of a budget (e.g., active/inactive).

### **Categories**

- **POST** `/api/Categories`: Create a new category.
- **GET** `/api/Categories/CategoriesType`: Fetch available category types.
- **GET** `/api/Categories/List`: Fetch a list of all categories.
- **GET** `/api/Categories/{id}`: Fetch a specific category by its ID.
- **PUT** `/api/Categories/{id}`: Update a specific category by its ID.
- **DELETE** `/api/Categories/{id}`: Delete a specific category by its ID.
- **PUT** `/api/Categories/ChangeState`: Change the state of a category.

### **Expenses**

- **GET** `/api/Expenses/List/{budgetId}`: Fetch a list of expenses for a specific budget.
- **GET** `/api/Expenses`: Fetch a list of all expenses.
- **POST** `/api/Expenses`: Create a new expense.
- **GET** `/api/Expenses/{id}`: Fetch a specific expense by its ID.
- **PUT** `/api/Expenses/{id}`: Update a specific expense by its ID.
- **DELETE** `/api/Expenses/{id}`: Delete a specific expense by its ID.

### **Finance Evaluation**

- **GET** `/api/FinanceEvaluation/List`: Fetch a list of finance evaluations.
- **GET** `/api/FinanceEvaluation`: Fetch finance evaluation details.
- **POST** `/api/FinanceEvaluation`: Create a new finance evaluation.
- **GET** `/api/FinanceEvaluation/{id}`: Fetch a specific finance evaluation by its ID.
- **PUT** `/api/FinanceEvaluation/{id}`: Update a specific finance evaluation by its ID.
- **DELETE** `/api/FinanceEvaluation/{id}`: Delete a specific finance evaluation by its ID.
- **PUT** `/api/FinanceEvaluation/ChangeState`: Change the state of a finance evaluation.

### **General**

- **GET** `/api/General/TotalPages`: Fetch pagination information for different tables.

### **Location**

- **GET** `/api/Location/List`: Fetch a list of locations.
- **GET** `/api/Location/{id}`: Fetch a specific location by its ID.
- **PUT** `/api/Location/{id}`: Update a specific location by its ID.
- **DELETE** `/api/Location/{id}`: Delete a specific location by its ID.
- **POST** `/api/Location`: Create a new location.
- **PUT** `/api/Location/ChangeState`: Change the state of a location.

### **Payment Methods**

- **GET** `/api/PaymentMethods`: Fetch a list of payment methods.
- **POST** `/api/PaymentMethods`: Create a new payment method.
- **GET** `/api/PaymentMethods/{id}`: Fetch a specific payment method by its ID.
- **PUT** `/api/PaymentMethods/{id}`: Update a specific payment method by its ID.
- **DELETE** `/api/PaymentMethods/{id}`: Delete a specific payment method by its ID.

### **Shopping**

- **GET** `/api/Shopping`: Fetch shopping data.
- **POST** `/api/Shopping`: Create a new shopping record.
- **GET** `/api/Shopping/List`: Fetch a list of shopping records.
- **GET** `/api/Shopping/Clone`: Clone an existing shopping record.
- **GET** `/api/Shopping/{id}`: Fetch a specific shopping record by its ID.
- **PUT** `/api/Shopping/{id}`: Update a specific shopping record by its ID.
- **DELETE** `/api/Shopping/{id}`: Delete a specific shopping record by its ID.
- **PUT** `/api/Shopping/ChangeState`: Change the state of a shopping record.


## Contributing

If you'd like to contribute to this project, feel free to fork the repository and submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Thanks to the Vue.js community for their continuous support and resources.
- Special thanks to Bootstrap for providing a solid foundation for responsive design.
```

Let me know if you'd like to add or change anything!