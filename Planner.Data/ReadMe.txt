Generate existing dbmodels:
dotnet ef dbcontext scaffold  "Server=servername;Database=Planner;Integrated Security = True" Microsoft.EntityFrameworkCore.SqlServer -o Model -d

Use this = > dotnet ef dbcontext scaffold  "Data Source=servername;Initial Catalog=Planner;;Integrated Security=SSPI; MultipleActiveResultSets=true;" -s ../Planner.API/Planner.csproj Microsoft.EntityFrameworkCore.SqlServer -o Model -d


