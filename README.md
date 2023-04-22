# RuData API


### Добавление NuGet-пакетов:

```
dotnet add package Microsoft.AspNet.WebApi.Client
dotnet add package Efir.DataHub.Models --source http://developer.efir-net.ru/nuget
```

### Зависимости

- Microsoft.AspNet.WebApi.Client
- Efir.DataHub.Models ([подробнее](https://developer.efir-net.ru/NuGetFeed))

### Использование

```csharp
var credentials = EfirClient.GetCredentialsFromFile("MyCredentils.json");
using EfirClient efir = new EfirClient(credentials);

await efir.LoginAsync();

var secinfo = await efir.GetSecurityData("RU000A105DL4");

Console.WriteLine(secinfo.nickname);
Console.WriteLine(secinfo.endmtydate);
Console.WriteLine(secinfo.fintoolid);
```