```cmd
dotnet ef migrations add AddOrderModel --startup-project .\AuthenticationServices\ --project .\Shared\

dotnet ef database update --project .\AuthenticationServices\
```