use QuickFinanceDB

SELECT *
FROM Categories

SELECT *
FROM PaymentMethods 

SELECT *
FROM Locations

SELECT * 
FROM Budgets B
LEFT OUTER JOIN Expenses E ON B.ID = E.BudgetId
LEFT OUTER JOIN PaymentMethods P ON E.PaymentMethodId = P.Id

SELECT *
FROM Shoppings S
LEFT OUTER JOIN ShoppingLists SL ON S.ID = SL.ShoppingId
LEFT OUTER JOIN locations l on sl.locationid = l.id	

SELECT *
FROM FinanceEvaluations F
LEFT OUTER JOIN FinanceDetails FD ON F.ID = FD.FinanceId
LEFT OUTER JOIN FinanceIncomes FI on F.Id = FI.FinanceId