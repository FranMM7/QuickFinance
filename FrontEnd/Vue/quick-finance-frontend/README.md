```markdown
# QuickFinance Frontend

## Overview

QuickFinance is a personal finance tracking application that helps users manage their budgets, expenses, and categories efficiently. This frontend application is built using Vue.js, Bootstrap, and Vite for a responsive and interactive user experience.

## Features

- User-friendly interface for managing budgets and expenses.
- Display and edit categories and expenses.
- Responsive design with Bootstrap styling.
- Axios for API calls to the backend.
- State management using Vuex.

## Tech Stack

- **Vue.js**: A progressive JavaScript framework for building user interfaces.
- **Bootstrap**: A popular CSS framework for developing responsive and mobile-first web pages.
- **Bootswatch**: Free themes for Bootstrap
- **Vite**: A modern frontend build tool for faster development and hot module replacement.
- **Axios**: A promise-based HTTP client for making API requests.
- **Vuex**: A state management pattern + library for Vue.js applications.

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/frankmejia7/QuickFinance-Frontend.git
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
- Use the **Budgets**, **Categories**, and **Expenses** sections to manage your financial data.
- Use the **Settings** section to configure your preferences.

## API Endpoints

This frontend interacts with the following backend API endpoints:

- **Expenses**
  - `GET /api/Expenses/List/{budgetId}?PageNumber=${PageNumber}&RowsPage=${RowsPage}`: Fetch expenses based on the selected budget.
- **Categories**
  - `GET /api/Categories/List?PageNumber=${PageNumber}&RowsPage=${RowsPage}`: Fetch category List.
- **Budgets**
  - `GET /api/budgets/List?PageNumber=${PageNumber}&RowsPage=${RowsPage}`: Fetch Budget List.
- **Payment methods**
  - `GET /api/PaymentMethods/List?PageNumber=${PageNumber}&RowsPage=${RowsPage}`: Fetch Payments List.
- **General**
  - `GET /api/General/TotalPages?RowsPage=${RowsPage}&tableName=${Table}`: Fetch retrives the pagination info.

## Contributing

If you'd like to contribute to this project, feel free to fork the repository and submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Thanks to the Vue.js community for their continuous support and resources.
- Special thanks to Bootstrap for providing a solid foundation for responsive design.

Feel free to add more sections if needed, such as a FAQ or troubleshooting guide. Let me know if you need any modifications or additional information!