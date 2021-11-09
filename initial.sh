dotnet new webapi -o AspExample
dotnet new xunit -o AspExample.Test
dotnet new sln
dotnet sln add AspExample
dotnet sln add AspExample.Test
dotnet add AspExample.Test/AspExample.Test.csproj reference AspExample
cd AspExample
dotnet add package Microsoft.EntityFrameworkCore.Sqlite -v 3.1

