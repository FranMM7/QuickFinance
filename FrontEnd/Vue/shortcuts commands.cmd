//to generate a file with all the directory context for github or other tools 
Get-ChildItem -Recurse -File | Where-Object { $_.FullName -notlike '*node_modules*' } | Out-File project_structure.txt


# Get all files recursively, excluding certain directories
Get-ChildItem -Recurse -File | 
Where-Object { 
    $_.FullName -notlike '*node_modules*' -and 
    $_.FullName -notlike '*obj*' -and 
    $_.FullName -notlike '*Migrations*' -and 
    $_.FullName -notlike '*bin*' 
} |
# Format the output into a structured hierarchy
ForEach-Object { 
    # Replace backslashes with forward slashes to ensure cross-platform readability
    $_.FullName.Replace((Get-Location).Path, "").Replace("\", "/")
} |
# Export to a text file
Out-File -FilePath project_structure.txt -Encoding utf8

# Optional: Print success message
Write-Output "Project structure saved to project_structure.txt"



//to generate mutliple files and expecific direcotries 
New-Item -ItemType Directory -Path src/components/Budgets, src/components/Categories, src/components/Expenses; New-Item -ItemType File -Path src/components/Budgets/AddBudget.vue; New-Item -ItemType File -Path src/components/Categories/CategoryList.vue; New-Item -ItemType File -Path src/components/Expenses/AddExpense.vue


mkdir src/components/Navbar && echo. > src/components/Navbar/Navbar.vue


New-Item -ItemType DIrectory -Path src/components/Navbar; New-Item -ItemType File -Path src/components/Navbar/Navbar.vue

mkdir src/views && echo. > src/views/HomeView.vue && echo. > src/views/BudgetView.vue && echo. > src/views/CategoryView.vue && echo. > src/views/ExpenseView.vue && echo. > src/views/SettingsView.vue

New-Item -ItemType File - Path src/components/Expenses/ExpensesView.vue;New-Item -ItemType File - Path src/components/Expenses/SettingsView.vue;

/*
vue-content-louder components
  components: {
    ContentLoader,
    FacebookLoader,
    CodeLoader,
    BulletListLoader,
    InstagramLoader,
    ListLoader, // Register ContentLoader component
  },
*/



//to run backend withou visual studio
C:\Code\QuickFinance\BackEnd\QuickFinance.Api
dotnet run
cd "C:\Code\QuickFinance\BackEnd\QuickFinance.Api"
dotnet run

//run frontend 
C:\Code\QuickFinance\FrontEnd\Vue\quick-finance-frontend

cd "C:\Code\QuickFinance\FrontEnd\Vue\quick-finance-frontend"
npm run dev
