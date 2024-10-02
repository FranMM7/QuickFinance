//to generate a file with all the directory context for github or other tools 
Get-ChildItem -Recurse -File | Where-Object { $_.FullName -notlike '*node_modules*' } | Out-File project_structure.txt


//to generate mutliple files and expecific direcotries 
New-Item -ItemType Directory -Path src/components/Budgets, src/components/Categories, src/components/Expenses; New-Item -ItemType File -Path src/components/Budgets/AddBudget.vue; New-Item -ItemType File -Path src/components/Categories/CategoryList.vue; New-Item -ItemType File -Path src/components/Expenses/AddExpense.vue


mkdir src/components/Navbar && echo. > src/components/Navbar/Navbar.vue


New-Item -ItemType DIrectory -Path src/components/Navbar; New-Item -ItemType File -Path src/components/Navbar/Navbar.vue

mkdir src/views && echo. > src/views/HomeView.vue && echo. > src/views/BudgetView.vue && echo. > src/views/CategoryView.vue && echo. > src/views/ExpenseView.vue && echo. > src/views/SettingsView.vue

New-Item -ItemType File - Path src/components/Expenses/ExpensesView.vue;New-Item -ItemType File - Path src/components/Expenses/SettingsView.vue;


//to run backend withou visual studio
C:\Code\QuickFinance\BackEnd\QuickFinance.Api 
dotnet run
